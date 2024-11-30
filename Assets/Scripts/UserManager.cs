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
    public TMP_InputField userIdInput;
    public TMP_InputField passwordInput;
    public TMP_InputField passwordConfirmInput;
    public Button signUpButton;
    public NetworkManager networkManager;

    void Start()
    {
        signUpButton.onClick.AddListener(() => StartCoroutine(SignUpCoroutine()));
    }

    private IEnumerator SignUpCoroutine()
    {
        // inputfield 값 가져오기
        string username = usernameInput.text;
        string userId = userIdInput.text;
        string password = passwordInput.text;
        string passwordConfirm = passwordConfirmInput.text;
        
        // DTO 따라 JSON
        SignUpDTO signUpData = new SignUpDTO
        {
            username = usernameInput.text,
            userId = userIdInput.text,
            password = passwordInput.text,
            passwordConfirm = passwordConfirmInput.text
        };
        string json = JsonUtility.ToJson(signUpData);
        string endPoint = "sign-up";

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
}