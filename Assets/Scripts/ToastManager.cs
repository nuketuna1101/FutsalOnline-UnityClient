using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum MSG_TYPE
{
    INFO,
    ERROR,
}

public class ToastManager : MonoBehaviour
{
    public static ToastManager Instance;

    public TextMeshProUGUI toastText;
    public CanvasGroup canvasGroup;
    public float duration = 2f;

    private void Awake()
    {
        // 싱글턴 패턴
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

    public void ShowToast(string message, MSG_TYPE msgType = MSG_TYPE.INFO)
    {
        StartCoroutine(ShowToastCoroutine(message, msgType));
    }

    private IEnumerator ShowToastCoroutine(string message, MSG_TYPE msgType = MSG_TYPE.INFO) 
    {
        // 메시지와 메시지 유형 설정
        toastText.text = message;
        switch (msgType)
        {
            case MSG_TYPE.INFO:
                toastText.color = Color.yellow;
                break;
            case MSG_TYPE.ERROR:
                toastText.color = Color.red;
                break;
            default:
                toastText.color = Color.yellow;
                break;
        }
        // 메시지 표시
        canvasGroup.alpha = 1f; 
        toastText.gameObject.SetActive(true);
        // 메시지 표시 시간 대기
        yield return new WaitForSeconds(duration);
        // 페이드 아웃 시간
        for (float t = 0; t < 1f; t += Time.deltaTime / 0.5f) 
        {
            canvasGroup.alpha = 1f - t;
            yield return null;
        }

        toastText.gameObject.SetActive(false);
    }
}
