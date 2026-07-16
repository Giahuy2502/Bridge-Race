using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : Singleton<UIController>
{
    private UIManager UIManager => UIManager.Instance;

    public void ShowMenu()
    {
        UIManager.Open<CanvasMainMenu>();
    }

    public void HideMenu(float duration)
    {
        UIManager.CloseUI<CanvasMainMenu>(duration);
    }

    public void ShowWinMenu()
    {
        HideGamePlay(0f);
        UIManager.Open<CanvasVictory>();
    }
    public void HideGamePlay(float duration)
    {
        UIManager.CloseUI<CanvasGamePlay>(duration);
    }
}
