using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class LobbyManager : MonoBehaviour
{
    public TransitionManager transitionManager;  // TransitionManager 할당
    [Header("Refresh")]
    public Button refreshButton;
    [Header("Gatcha")]
    public Button gatchaButton;
    [Header("Lobby User Data Display")]
    public TextMeshProUGUI textUserNickname;
    public TextMeshProUGUI textRating;
    public TextMeshProUGUI textRank;


    void Start()
    {
        if (refreshButton != null)
            refreshButton.onClick.AddListener(() => StartCoroutine(RefreshCoroutine()));
        else
            Debug.Log("Refresh Button is not assigned in the Inspector!");


        if (gatchaButton != null)
            gatchaButton.onClick.AddListener(() => StartCoroutine(GatchaCoroutine()));
        else
            Debug.Log("Gatcha Button is not assigned in the Inspector!");

        // 최초에 리프레시
        UserManager.Instance.RefreshData();
    }
    private IEnumerator RefreshCoroutine()
    {
        UserManager.Instance.RefreshData();
        yield return null;
    }

    private IEnumerator GatchaCoroutine()
    {
        // inputfield 값 가져오기

        string json = "{}";
        string endPoint = "gatcha";

        NetworkManager.Instance.PostRequest(endPoint, json, (response) => {
            if (response == null)
            {
                string msg = "[Error] :: gatcha failed.";
                Debug.LogError(msg);
                ToastManager.Instance.ShowToast(msg, MSG_TYPE.ERROR);
            }
            else
            {
                // 서버 응답을 처리
                try
                {
                    GatchaResponse gatchaResponse = JsonUtility.FromJson<GatchaResponse>(response);
                    string msg = gatchaResponse.Message;
                    Debug.Log(msg);
                    ToastManager.Instance.ShowToast(msg);
                }
                catch (System.Exception ex)
                {
                    Debug.LogError("[Error] :: Failed to parse Gatcha response. " + ex.Message);
                    ToastManager.Instance.ShowToast("[Error] :: Gatcha response parsing failed.", MSG_TYPE.ERROR);
                }
            }
        });

        yield return null;
    }


    private void Update()
    {
        UpdateUserDataDisplay();
    }

    private void UpdateUserDataDisplay()
    {
        // 유저 데이터 업데이트
        if (UserManager.Instance.IsLoggedIn)
        {
            textUserNickname.text = UserManager.Instance.UserNickname;
            textRating.text = "Rating: " + UserManager.Instance.Rating.ToString();
            textRank.text = "Rank: " + UserManager.Instance.Rank.ToString();
        }
        else
        {
            textUserNickname.text = "Guest";
            textRating.text = "Rating: N/A";
            textRank.text = "Rank: N/A";
        }
    }


}
