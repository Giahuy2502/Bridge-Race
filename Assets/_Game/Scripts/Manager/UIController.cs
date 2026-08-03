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
    public void ShowMenu()
    {
        UIManager.Open<CanvasMainMenu>();
    }
    // couroutine show endgame canvas
    public IEnumerator ShowEndGameMenu(bool isWinGame)
    {
        canvasGamePlay.SetActivateSettingButton(false);
        HideJoyStick(0f);
        yield return new WaitForSeconds(endGameDuration);
        HideGamePlay(0f);
        if (isWinGame)
        {
            UIManager.Open<CanvasVictory>();
        }
        else
        {
            UIManager.Open<CanvasFail>();
        }
        canvasGamePlay.SetActivateSettingButton(true);
    }
    private void HideGamePlay(float duration)
    {
        UIManager.CloseUI<CanvasGamePlay>(duration);
    }
    private void HideJoyStick(float duration)
    {
        UIManager.CloseUI<CanvasInput>(duration);
    }
    public void ShowGamePlay()
    {
        canvasGamePlay = UIManager.Open<CanvasGamePlay>();
    }
    public void ShowJoyStick()
    {
        canvasInput = UIManager.Open<CanvasInput>();
    }
    // ham play countdown
    public void PlayCountdown()
    {
        canvasGamePlay.PlayCountDown();
    }
    public void ShowLoading()
    {
        UIManager.Open<CanvasLoading>();
    }
    // ham update color ranking khi co character vao stage moi
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

    public CanvasInput GetCanvasInput()
    {
        return canvasInput;
    }
}
