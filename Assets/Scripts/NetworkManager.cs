using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;


/// <summary>
/// NetworkManager : api 호출 전담하는 부분
/// </summary>

public class NetworkManager : MonoBehaviour
{
    const string baseUrl = "http://localhost:3019/api/";


    public void PostRequest(string endPoint, string json, System.Action<string> callback)
    {
        Debug.Log("start PostRequestCoroutine()");
        StartCoroutine(PostRequestCoroutine(endPoint, json, callback));
    }

    private IEnumerator PostRequestCoroutine(string endPoint, string json, System.Action<string> callback)
    {
        StringBuilder urlBuilder = new StringBuilder(baseUrl).Append(endPoint);
        string url = urlBuilder.ToString();

        UnityWebRequest www = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        www.uploadHandler = new UploadHandlerRaw(bodyRaw);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error: " + www.error);
            callback(null);
        }
        else
        {
            callback(www.downloadHandler.text);
        }
    }
}
