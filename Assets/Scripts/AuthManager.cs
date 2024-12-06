using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using System.Net;
using System;
using System.Transactions;
using UnityEngine.SceneManagement;
/// <summary>
/// AuthManager : 유저 회원 가입, 로그인 로직 처리
/// </summary>


public class AuthManager : MonoBehaviour
{
    public TransitionManager transitionManager;  // TransitionManager 할당
    [Header("SignUp")]
    public TMP_InputField usernameInput;
    public TMP_InputField nicknameInput;
    public TMP_InputField passwordInput;
    public TMP_InputField confirmPasswordInput;
    public Button signUpButton;

    [Header("SignIn")]
    public TMP_InputField nicknameSignInInput;
    public TMP_InputField passwordSignInInput;
    public Button signInButton;

    [Header("Ping")]
    private bool isServerRunning = false;
    private bool hasShownAliveToast = false; // "Server is running" 토스트를 최초 1회만 표시
    [SerializeField]
    private float pingInterval = 5f; // 핑 테스트 간격 (초 단위)

    void Start()
    {
        if (signUpButton != null)
            signUpButton.onClick.AddListener(() => StartCoroutine(SignUpCoroutine()));
        else
            Debug.Log("SignUp Button is not assigned in the Inspector!");

        if (signInButton != null)
            signInButton.onClick.AddListener(() => StartCoroutine(SignInCoroutine()));
        else
            Debug.Log("SignIn Button is not assigned in the Inspector!");

        StartCoroutine(PingTestCoroutine());
    }

    private IEnumerator SignUpCoroutine()
    {
        // inputfield 값 가져오기
        string username = usernameInput.text;
        string nickname = nicknameInput.text;
        string password = passwordInput.text;
        string confirmPassword = confirmPasswordInput.text;
        // DTO 따라 JSON
        SignUpDTO signUpData = new SignUpDTO
        {
            userName = usernameInput.text,
            nickname = nicknameInput.text,
            password = passwordInput.text,
            confirmPassword = confirmPasswordInput.text
        };
        string json = JsonUtility.ToJson(signUpData);
        string endPoint = "users/sign-up";

        NetworkManager.Instance.PostRequest(endPoint, json, (response) => {
            if (response == null)
            {
                string msg = "[Error] :: Sign-up failed.";
                Debug.LogError(msg);
                ToastManager.Instance.ShowToast(msg, MSG_TYPE.ERROR);
            }
            else
            {
                string msg = "[Success] :: Sign-up completed.";
                Debug.Log(msg);
                ToastManager.Instance.ShowToast(msg);
            }
        });

        yield return null;
    }


    private IEnumerator SignInCoroutine()
    {
        // inputfield 값 가져오기
        string nickname = nicknameSignInInput.text;
        string password = passwordSignInInput.text;
        
        // DTO 따라 JSON
        SignInDTO signInData = new SignInDTO
        {
            nickname = nickname,
            password = password,
        };

        string json = JsonUtility.ToJson(signInData);
        string endPoint = "users/sign-in";

        NetworkManager.Instance.PostRequest(endPoint, json, (response) => {
            if (response == null)
            {
                string msg = "[Error] :: Sign-in failed.";
                Debug.LogError(msg);
                ToastManager.Instance.ShowToast(msg, MSG_TYPE.ERROR);
            }
            else
            {
                // 서버 응답 처리
                SignInResponse signInResponse = JsonUtility.FromJson<SignInResponse>(response);

                // 서버에서 닉네임을 반환한 경우 파싱
                string receivedNickname = signInResponse.nickname ?? nickname;
                string receivedUserId = signInResponse.userId ?? "NO_ID";
                string accessToken = signInResponse.accessToken;  // 토큰 받기

                // 환영 메시지 표시
                string msg = $"[Success] :: Welcome, {receivedNickname}!";
                // 유저 로그인 저장
                UserManager.Instance.SetUserData(receivedUserId, receivedNickname, accessToken);
                Debug.Log(msg);
                ToastManager.Instance.ShowToast(msg);
                StartCoroutine(AfterSignInCoroutine());
            }
        });
        
        yield return null;
    }
    private IEnumerator AfterSignInCoroutine()
    {
        yield return new WaitForSeconds(1.0f);
        transitionManager.TransitionToScene("1.UserLobby");
        //SceneManager.LoadScene("1.UserLobby");
        yield return null;
    }


    private IEnumerator PingTestCoroutine()
    {
        while (true)
        {
            // 인터벌만큼 주기적 반복
            PingTest();
            yield return new WaitForSeconds(pingInterval);
        }
    }

    private void PingTest()
    {
        // 핑 테스트
        NetworkManager.Instance.GetRequest("ping", (response) => {
            // 핑 실패
            if (response == null)
            {
                if (!isServerRunning) return;

                isServerRunning = false; 
                hasShownAliveToast = false;
                string msg = "[Ping failed] :: Server NOT running.";
                Debug.Log(msg);
                ToastManager.Instance.ShowToast(msg, MSG_TYPE.ERROR);
            }
            // 핑 성공
            else
            {
                if (!isServerRunning) isServerRunning = true;
                // 최초 1회만 보여주기 위해
                if (hasShownAliveToast) return;
                hasShownAliveToast = true;
                string msg = "[Ping Test] Server is running!";
                Debug.Log(msg);
                ToastManager.Instance.ShowToast(msg);

            }
        });
    }
}
