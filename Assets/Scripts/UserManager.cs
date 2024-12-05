using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using System.Net;
/// <summary>
/// UserManager : 유저 회원 가입, 로그인 로직 처리
/// </summary>

public class UserManager : MonoBehaviour
{
    public TMP_InputField usernameInput;
    public TMP_InputField nicknameInput;
    public TMP_InputField passwordInput;
    public TMP_InputField confirmPasswordInput;
    public Button signUpButton;
    public NetworkManager networkManager;
    private bool isServerRunning = false;

    void Start()
    {
        signUpButton.onClick.AddListener(() => StartCoroutine(SignUpCoroutine()));
        PingTest();
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

        networkManager.PostRequest(endPoint, json, (response) => {
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

    private void PingTest()
    {
        // 핑 테스트
        networkManager.GetRequest("ping", (response) => {
            if (response == null)
            {
                string msg = "[Ping failed] :: Server NOT running.";
                Debug.Log(msg);
                ToastManager.Instance.ShowToast(msg, MSG_TYPE.ERROR);
            }
            else
            {
                string msg = "[Ping Test] Server is running!";
                Debug.Log(msg);
                ToastManager.Instance.ShowToast(msg);
            }
        });
    }
}


