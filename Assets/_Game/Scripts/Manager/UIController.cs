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
    private SoundManager SoundManager => SoundManager.Instance;
    private DataManager DataManager => DataManager.Instance;
    private LevelManager LevelManager => LevelManager.Instance;
    private GameManager GameManager => GameManager.Instance;
    public void ShowMenu()
    {
        UIManager.Open<CanvasMainMenu>();
    }
    // couroutine show endgame canvas
    public IEnumerator ShowEndGameMenu(bool isWinGame)
    {
        canvasGamePlay.SetActivateSettingButton(false);
        if (!RankManager.IsPlayerWinner())
        {
            canvasGamePlay.SetActivateEndGameNotifi(true);
            HideJoyStick(0f); 
            yield return new WaitForSeconds(endGameDuration/2);
            RankManager.SetFinishedPosition();
            GameManager.SetEndCam();
            yield return new WaitForSeconds(endGameDuration/2);
        }
        else
        {
            HideJoyStick(0f); 
            RankManager.SetFinishedPosition();
            GameManager.SetEndCam();
            yield return new WaitForSeconds(endGameDuration);
        }
        HideGamePlay(0f);
        if (isWinGame)
        {
            int numStar = RankManager.GetCountStar(RankManager.GetPlayerRanking());
            UIManager.Open<CanvasVictory>().SetStar(numStar);
            SoundManager.PlayFx(FxID.SFX_Win);
            SoundManager.ChangeSound(SoundID.BG_MainMenu,1f);
            DataManager.SetStartLevel(LevelManager.GetCurrentLevel(),numStar);
            DataManager.UnlockLevel(LevelManager.GetCurrentLevel()+1);
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
        canvasGamePlay.OnInit(LevelManager.GetCurrentLevel(), GameManager.GetPlayerColor());
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
