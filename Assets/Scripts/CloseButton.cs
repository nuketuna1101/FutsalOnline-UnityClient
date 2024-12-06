using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloseButton : MonoBehaviour
{
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        // 부모 오브젝트를 비활성화
        if (transform.parent != null)
        {
            transform.parent.gameObject.SetActive(false);
        }
    }
}
