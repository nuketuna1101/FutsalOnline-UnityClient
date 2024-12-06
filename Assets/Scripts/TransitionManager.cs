using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class TransitionManager : MonoBehaviour
{
    public CanvasGroup fadeCanvasGroup;
    public float fadeDuration = 1f;

    private void Start()
    {
        // 초기 상태: 완전히 투명하게 설정
        fadeCanvasGroup.alpha = 1;

        // 씬 전환 시 이 오브젝트를 파괴하지 않도록 설정
        DontDestroyOnLoad(gameObject);
    }

    public void TransitionToScene(string sceneName)
    {
        // 페이드 아웃
        fadeCanvasGroup.DOFade(0, fadeDuration).OnComplete(() =>
        {
            // 씬 로드
            SceneManager.LoadScene(sceneName);

            // 씬 전환 후 페이드 인
            fadeCanvasGroup.DOFade(1, fadeDuration);
        });
    }
}
