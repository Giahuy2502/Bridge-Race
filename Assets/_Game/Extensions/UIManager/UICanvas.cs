using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class UICanvas : MonoBehaviour
{
    [SerializeField] private bool isDestroyOnClose = false;

    // xu ly tai tho
    private void Awake()
    {
        RectTransform rect  = GetComponent<RectTransform>();
        float ratio = (float)Screen.width / (float)Screen.height;
        if (ratio > 2.1f)
        {
            Vector2 leftButtom = rect.offsetMin;
            Vector2 rightTop = rect.offsetMax;

            leftButtom.y = 0;
            rightTop.y = -100f;
            
            rect.offsetMin = leftButtom;
            rect.offsetMax = rightTop;
        }
    }

    // goi trc khi canvas duoc active
    public virtual void Setup()
    {
        
    }

    //goi sa khi duoc activate
    public virtual void Open()
    {
        gameObject.SetActive(true);
    }

    //dong canvas sau bn giay
    public virtual void Close(float time)
    {
        Invoke(nameof(CloseDirectly),time);
    }

    // dong canvas truc tiep
    public virtual void CloseDirectly()
    {
        if (isDestroyOnClose)
        {
            Destroy(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
