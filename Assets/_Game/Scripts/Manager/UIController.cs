using System.Collections;
using System.Collections.Generic;
using MyNamespace;
using UnityEngine;

public class UIController : Singleton<UIController>
{
    [SerializeField] private float victoryDuration = 1f;
    private List<ColorType> colorRanking = new List<ColorType>();
    private CanvasGamePlay canvasGamePlay;
    private UIManager UIManager => UIManager.Instance;
    private RankManager RankManager => RankManager.Instance;
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
        yield return new WaitForSeconds(victoryDuration);
        HideGamePlay(0f);
        HideJoyStick(0f);
        UIManager.Open<CanvasVictory>();
    }
    public void HideGamePlay(float duration)
    {
        UIManager.CloseUI<CanvasGamePlay>(duration);
    }

    public void HideJoyStick(float duration)
    {
        UIManager.CloseUI<CanvasInput>(duration);
    }

    public void ShowGamePlay()
    {
        canvasGamePlay = UIManager.Open<CanvasGamePlay>();
        UIManager.Open<CanvasInput>();
    }

    public void UpdateColorRanking()
    {
        if (canvasGamePlay == null)
        {
            Debug.LogError("canvasGamePlay is null");
            return;
        }
        colorRanking.Clear();
        colorRanking = RankManager.GetCurrentColorRanking();
        canvasGamePlay.UpdateColorRanking(colorRanking);
    }
}
