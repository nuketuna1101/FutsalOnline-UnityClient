using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitButton : MonoBehaviour
{
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
    #if UNITY_EDITOR
        // Unity 에디터에서 테스트 중일 때는 게임 종료가 아닌 플레이 모드를 멈추게 함
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        // 실제 빌드된 게임에서는 애플리케이션 종료
        Application.Quit();
    #endif
    }
}
