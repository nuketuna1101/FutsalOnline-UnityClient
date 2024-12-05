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
            callback(www.downloadHandler.text);
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


}