using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
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
    public Button testButton;
    public NetworkManager networkManager;

    void Start()
    {
        signUpButton.onClick.AddListener(() => StartCoroutine(SignUpCoroutine()));
        testButton.onClick.AddListener(() => StartCoroutine(TestCoroutine()));
    }

    private IEnumerator SignUpCoroutine()
    {
        // inputfield 값 가져오기
        string username = usernameInput.text;
        string nickname = nicknameInput.text;
        string password = passwordInput.text;
        string confirmPassword = confirmPasswordInput.text;
        Debug.Log("username : " + username);
        Debug.Log("userId : " + nickname);
        Debug.Log("password : " + password);
        Debug.Log("passwordConfirm : " + confirmPassword);
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
                Debug.LogError("Sign-up failed.");
            }
            else
            {
                Debug.Log("Sign-up successful: " + response);
            }
        });

        yield return null;
    }

    private IEnumerator TestCoroutine()
    {
        // inputfield 값 가져오기
        ToastManager.Instance.ShowToast("Test: test text" );
        yield return null;
    }
}


