using System.Collections;
using System.Collections.Generic;
using MyNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CanvasVictory : UICanvas
{
    [SerializeField] private List<Image> starts;
    private int numStar = 0;
    private GameManager GameManager => GameManager.Instance;
    private RankManager RankManager => RankManager.Instance;
    private DataManager DataManager => DataManager.Instance;
    private LevelManager LevelManager => LevelManager.Instance;
    private SoundManager SoundManager => SoundManager.Instance;

    public override void Setup()
    {
        base.Setup();
        SoundManager.PlayFx(FxID.SFX_Win);
        SoundManager.ChangeSound(SoundID.BG_MainMenu,1f);
        numStar = GetCountStar(RankManager.GetPlayerRanking());
        SetStar(numStar);
        DataManager.SetStartLevel(LevelManager.GetCurrentLevel(),numStar);
        DataManager.UnlockLevel(LevelManager.GetCurrentLevel()+1);
    }
    public void MainMenuButton()
    {
        LevelManager.SetNextLevel();
        GameManager.OnMainMenu();
        Close(0);
        UIManager.Instance.Open<CanvasMainMenu>();
    }

    public void NextLevelButton()
    {
        GameManager.NextLevel();
        Close(0);
    }

    private void SetStar(int count)
    {
        DisableAllStars();
        for (int i = 0; i < count; i++)
        {
            starts[i].gameObject.SetActive(true);
        }
    }

    private int GetCountStar(int playerRank)
    {
        switch (playerRank)
        {
            case 0:
                return 3;
            case 1:
                return 2;
            case 2:
                return 1;
            default:
                return 0;
        }
    }

    private void DisableAllStars()
    {
        for (int i = 0; i < starts.Count; i++)
        {
            starts[i].gameObject.SetActive(false);
        }
    }
}
