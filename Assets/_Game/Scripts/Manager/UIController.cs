using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : Singleton<UIController>
{
    [SerializeField] private float victoryDuration = 1f;
    private UIManager UIManager => UIManager.Instance;

    public void ShowMenu()
    {
        UIManager.Open<CanvasMainMenu>();
    }

    public void HideMenu(float duration)
    {
        UIManager.CloseUI<CanvasMainMenu>(duration);
    }
    public IEnumerator ShowWinMenu()
    {
        HideGamePlay(0f);
        yield return new WaitForSeconds(victoryDuration);
        UIManager.Open<CanvasMainMenu>();
    }
    public void HideGamePlay(float duration)
    {
        UIManager.CloseUI<CanvasGamePlay>(duration);
    }
}
