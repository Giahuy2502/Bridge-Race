using System.Collections;
using System.Collections.Generic;
using MyNamespace;
using UnityEngine;

public class UIController : Singleton<UIController>
{
    [SerializeField] private float endGameDuration = 1f;
    private List<ColorType> colorRanking = new List<ColorType>();
    private CanvasGamePlay canvasGamePlay;
    private CanvasInput canvasInput;
    private UIManager UIManager => UIManager.Instance;
    private RankManager RankManager => RankManager.Instance;
    private GameManager GameManager => GameManager.Instance;
    public CanvasInput CanvasInput => canvasInput;
    public void ShowMenu()
    {
        UIManager.Open<CanvasMainMenu>();
    }

    public void HideMenu(float duration)
    {
        UIManager.CloseUI<CanvasMainMenu>(duration);
    }
    public IEnumerator ShowEndGameMenu(bool isWinGame)
    {
        yield return new WaitForSeconds(endGameDuration);
        HideGamePlay(0f);
        HideJoyStick(0f);
        if (isWinGame)
        {
            UIManager.Open<CanvasVictory>();
        }
        else
        {
            UIManager.Open<CanvasFail>();
        }
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
        canvasInput = UIManager.Open<CanvasInput>();
    }

    public void ShowLoading()
    {
        UIManager.Open<CanvasLoading>();
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
