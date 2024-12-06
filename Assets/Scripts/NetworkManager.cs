using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;


/// <summary>
/// NetworkManager : api 호출 전담하는 부분
/// </summary>

public class NetworkManager : MonoBehaviour
{
    const string baseUrl = "http://localhost:3321/api/";
    private string authToken;
    public static NetworkManager Instance { get; private set; }

    private void Awake()
    {
        // 싱글턴 패턴 적용
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // 기존 인스턴스가 있으면 삭제
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // 씬 전환 시에도 파괴되지 않도록 설정
    }


    public void PostRequest(string endPoint, string json, System.Action<string> callback)
    {
        // POST 메서드 처리
        StartCoroutine(PostRequestCoroutine(endPoint, json, callback));
    }

    private IEnumerator PostRequestCoroutine(string endPoint, string json, System.Action<string> callback)
    {
        // url: 기본 포트 + 앤드포인트로 지정
        StringBuilder urlBuilder = new StringBuilder(baseUrl).Append(endPoint);
        string url = urlBuilder.ToString();
        // serialize 
        UnityWebRequest www = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        www.uploadHandler = new UploadHandlerRaw(bodyRaw);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");
        // 서버에 req 전송, res 대기
        yield return www.SendWebRequest();
        // res 처리 로직
        if (www.result != UnityWebRequest.Result.Success)
        {
            callback(null);
        }
        else
        {
            // 응답 데이터가 UTF-8로 인코딩되어 제대로 처리되도록 변환
            string responseString = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
            callback(responseString);
        }
    }

    public void PostRequest(string endPoint, string json, string authToken, System.Action<string> callback)
    {
        StartCoroutine(PostRequestCoroutine(endPoint, json, authToken, callback));
    }

    private IEnumerator PostRequestCoroutine(string endPoint, string json, string authToken, System.Action<string> callback)
    {
        StringBuilder urlBuilder = new StringBuilder(baseUrl).Append(endPoint);
        string url = urlBuilder.ToString();
        UnityWebRequest www = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        www.uploadHandler = new UploadHandlerRaw(bodyRaw);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");
        // 인증 토큰의 추가
        www.SetRequestHeader("Authorization", "Bearer " + authToken);

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error: " + www.error);
            callback(null);
        }
        else
        {
            // 응답 데이터가 UTF-8로 인코딩되어 제대로 처리되도록 변환
            string responseString = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
            callback(responseString);
        }
    }


    public void GetRequest(string endPoint, System.Action<string> callback)
    {
        StartCoroutine(GetRequestCoroutine(endPoint, callback));
    }

    private IEnumerator GetRequestCoroutine(string endPoint, System.Action<string> callback)
    {
        // url: 기본 포트 + 앤드포인트로 지정
        StringBuilder urlBuilder = new StringBuilder(baseUrl).Append(endPoint);
        string url = urlBuilder.ToString();

        // GET 요청 생성
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        // res 처리 로직
        if (www.result != UnityWebRequest.Result.Success)
        {
            callback(null);
        }
        else
        {
            callback(www.downloadHandler.text);
        }
    }


    public void GetRequest(string endPoint, string authToken, System.Action<string> callback)
    {
        StartCoroutine(GetRequestCoroutine(endPoint, authToken, callback));
    }

    private IEnumerator GetRequestCoroutine(string endPoint, string authToken, System.Action<string> callback)
    {
        // url: 기본 포트 + 앤드포인트로 지정
        StringBuilder urlBuilder = new StringBuilder(baseUrl).Append(endPoint);
        string url = urlBuilder.ToString();

        // GET 요청 생성
        UnityWebRequest www = UnityWebRequest.Get(url);

        // 인증 헤더 추가
        if (!string.IsNullOrEmpty(authToken))
        {
            www.SetRequestHeader("Authorization", $"Bearer {authToken}");
        }

        yield return www.SendWebRequest();

        // res 처리 로직
        if (www.result != UnityWebRequest.Result.Success)
        {
            callback(null);
        }
        else
        {
            callback(www.downloadHandler.text);
        }
    }
}