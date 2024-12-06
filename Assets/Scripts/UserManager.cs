using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using System.Net;
/// <summary>
/// UserManager : 유저 상태, 토큰 관리
/// </summary>

public class UserManager : MonoBehaviour
{
    public static UserManager Instance { get; private set; }

    public string UserId { get; private set; }
    public string UserNickname { get; private set; }
    public string AccessToken { get; private set; }
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
}