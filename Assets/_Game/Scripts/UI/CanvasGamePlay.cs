using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanvasGamePlay : UICanvas
{
    [SerializeField] private TextMeshProUGUI coinText;

    public override void Setup()
    {
        base.Setup();
        UpdateCoin(0);
    }

    public void UpdateCoin(int coin)
    {
        coinText.text = coin.ToString();
    }

    public void SettingButton()
    {
        UIManager.Instance.Open<CanvasSettings>().SetState(this);
    }
}
