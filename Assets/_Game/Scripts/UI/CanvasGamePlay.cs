using System;
using System.Collections;
using System.Collections.Generic;
using MyNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CanvasGamePlay : UICanvas
{
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Button settingButton;
    [SerializeField] private List<RankingItemUI> rankingItemUI = new List<RankingItemUI>();
    [SerializeField] private ColorDataSO colorDataSO;
    [SerializeField] private CountDown countDown;

    public void OnInit(int currentLevel, ColorType playerColorType)
    {
        UpdateLevelText(currentLevel);
        SetupRankingItems(playerColorType);
    }
    // chay count down
    public void PlayCountDown()
    {
        countDown.OnInit();
    }
    // khoi tao bang xep hang
    private void SetupRankingItems(ColorType playerColor)
    {
        for (int i = 0; i < rankingItemUI.Count; i++)
        {
            rankingItemUI[i].OnInit(playerColor);
        }
    }

    public void SettingButton()
    {
        UIManager.Instance.Open<CanvasSettings>().SetState(this);
    }
    // cap nhat bang xep hang
    public void UpdateColorRanking(List<ColorType> colorRanking)
    {
        for (int i = 0; i < rankingItemUI.Count; i++)
        {
            RankingItemUI rankingItem = rankingItemUI[i];
            if (i >= colorRanking.Count)
            {
               return;
            }
            ColorType color = colorRanking[i];
            Material material = colorDataSO.GetMat(color);
            rankingItem.SetColor(color,material);
        }
    }
    // cap nhat level text
    private void UpdateLevelText(int level)
    {
        levelText.text = "Level "+ level.ToString();
        return;
    }
    // bat/tat setting button
    public void SetActivateSettingButton(bool isActivate)
    {
        settingButton.gameObject.SetActive(isActivate);
    }
}

