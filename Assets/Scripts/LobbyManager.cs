using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEditor.PackageManager.Requests;


public class LobbyManager : MonoBehaviour
{
    public TransitionManager transitionManager;  // TransitionManager 할당
    [Header("Refresh")]
    public Button refreshButton;
    [Header("Team")]
    public Button teamButton;
    [Header("Squad")]
    public Button squadButton;
    [Header("Gatcha")]
    public Button gatchaButton;
    [Header("Lobby User Data Display")]
    public TextMeshProUGUI textUserNickname;
    public TextMeshProUGUI textRating;
    public TextMeshProUGUI textRank;
    [Header("Lobby User Data Display")]
    public GameObject gobjPopup;
    [SerializeField] private TextMeshProUGUI textScrollViewPopup;


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

        if (teamButton != null)
            teamButton.onClick.AddListener(() => StartCoroutine(TeamCoroutine()));

        if (squadButton != null)
            squadButton.onClick.AddListener(() => StartCoroutine(SquadCoroutine()));

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



    private IEnumerator TeamCoroutine()
    {
        // inputfield 값 가져오기
        string endPoint = "players/my";

        if (!gobjPopup.activeSelf) gobjPopup.SetActive(true);

        NetworkManager.Instance.GetRequest(endPoint, UserManager.Instance.AccessToken, (response) => {
            if (response == null)
            {
                string msg = "[Error] :: view my players failed.";
                Debug.LogError(msg);
                ToastManager.Instance.ShowToast(msg, MSG_TYPE.ERROR);
            }
            else
            {
                // 성공한 경우 - 응답 처리
                try
                {
                    // 응답을 PlayerInfo 형식으로 파싱 (자세한 구조는 서버 응답에 따라 다름)
                    MyPlayersResponse playerResponse = JsonUtility.FromJson<MyPlayersResponse>(response);

                    if (playerResponse != null && playerResponse.data != null)
                    {
                        string tmp = "";
                        // 선수가 존재하는 경우
                        foreach (var player in playerResponse.data)
                        {
                            tmp += $"Player Name: {player.players.playerName}\n";
                            tmp += "Player Stats:\n";
                            tmp += $"- Technique: {player.players.playerStats.technique}\n";
                            tmp += $"- Pass: {player.players.playerStats.pass}\n";
                            tmp += $"- Pace: {player.players.playerStats.pace}\n";
                            tmp += $"- Agility: {player.players.playerStats.agility}\n";
                            tmp += $"- Defense: {player.players.playerStats.defense}\n";
                            tmp += $"- Finishing: {player.players.playerStats.finishing}\n";
                            tmp += $"- Stamina: {player.players.playerStats.stamina}\n";
                            tmp += "------------------------------\n";
                        }
                        textScrollViewPopup.text = tmp;
                    }
                    else
                    {
                        // 데이터가 없는 경우
                        Debug.LogError("[Error] :: No players found.");
                        ToastManager.Instance.ShowToast("[Error] :: No players found.", MSG_TYPE.ERROR);
                    }
                }
                catch (System.Exception ex)
                {
                    // 응답 파싱 오류 처리
                    Debug.LogError("[Error] :: Failed to parse response. " + ex.Message);
                    ToastManager.Instance.ShowToast("[Error] :: Failed to parse response.", MSG_TYPE.ERROR);
                }
            }
        });

        yield return null;
    }

    private IEnumerator SquadCoroutine()
    {
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
