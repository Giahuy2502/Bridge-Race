using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CanvasLoading : UICanvas
{
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private float duration = 2f;
    
    private float fillAmount = 0;
    private float timer = 0;
    
    private UIController UIController => UIController.Instance;

    public override void Setup()
    {
        base.Setup();
        timer = 0;
        UpdateFillAmount(0);
    }

    private void Update()
    {
        if (timer < duration)
        {
            timer += Time.deltaTime;
            fillAmount = Mathf.Clamp01(timer / duration);
            UpdateFillAmount(fillAmount);
            if (fillAmount >= 1f)
            {
                UIController.LoadingComplete();
            }
        }
    }

    private void UpdateFillAmount(float amount)
    {
        this.fillAmount = Mathf.Clamp01(amount);
        slider.value = this.fillAmount;
        text.text = (int)(this.fillAmount * 100) + "%"; 
    }
}