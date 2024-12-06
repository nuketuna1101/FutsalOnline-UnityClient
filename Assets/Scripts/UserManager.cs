using System.Collections;
using UnityEngine;

/// <summary>
/// UserManager : 유저 상태, 토큰 관리
/// </summary>

public class UserManager : MonoBehaviour
{
    public static UserManager Instance { get; private set; }

    public string UserId { get; private set; }
    public string UserNickname { get; private set; }
    public string AccessToken { get; private set; }
    public int Rating { get; private set; }
    public int Rank { get; private set; }
    public int Cash { get; private set; }

    public bool IsLoggedIn { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetUserData(string userId, string userNickname, string accessToken)
    {
        UserId = userId;
        UserNickname = userNickname;
        AccessToken = accessToken;
        IsLoggedIn = true;
    }

    public void ClearUserData()
    {
        UserId = null;
        UserNickname = null;
        AccessToken = null;
        IsLoggedIn = false;
    }

    public void RefreshData()
    {
        // refresh 버튼으로 누르기
        GetUserRatingandRank();
    }

    public void GetUserCash()
    {
        // 서버에 get 리퀘스트로 업데이트
    }

    public void GetUserRatingandRank()
    {
        // 서버에 get 리퀘스트로 업데이트
        StartCoroutine(GetUserRatingAndRankCoroutine());
    }


    private IEnumerator GetUserRatingAndRankCoroutine()
    {
        string endPoint = "users/ranks/" + UserId;

        NetworkManager.Instance.GetRequest(endPoint, (response) => {
            // 실패
            if (response == null)
            {
                string msg = "[Error] :: get ranks failed.";
                Debug.LogError(msg);
                ToastManager.Instance.ShowToast(msg, MSG_TYPE.ERROR);
            }
            // 조회 성공
            else
            {
                // 서버 응답 처리
                RankResponse rankResponse = JsonUtility.FromJson<RankResponse>(response);
                // 서버에서 닉네임을 반환한 경우 파싱
                string message = rankResponse.message;
                int userRank = rankResponse.userRank;
                int userRating = rankResponse.userRating;
                Debug.Log(message + userRank + userRating);
                Rating = userRating;
                Rank = userRank;
            }
        });
        yield return null;
    }
}