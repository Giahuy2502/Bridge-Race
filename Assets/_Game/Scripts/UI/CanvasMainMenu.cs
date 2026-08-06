using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasMainMenu : UICanvas
{
    [SerializeField] List<ButtonLevel> levels = new List<ButtonLevel>();
    private GameManager GameManager => GameManager.Instance;
    private LevelManager LevelManager => LevelManager.Instance;
    private DataManager DataManager => DataManager.Instance;

    public override void Setup()
    {
        base.Setup();
        SetupButtonLevels();
    }
    // ham play button
    public void PlayButton()
    {
        GameManager.PlayGame();
        Close(0);
    }
    // ham new game button
    public void NewGameButton()
    {
        GameManager.NewGame();
        Close(0);
    }
    // ham setting button
    public void SettingButton()
    {
        UIManager.Instance.Open<CanvasSettings>().SetState(this);
    }
    // ham shop button 
    public void ShopButton()
    {
        UIManager.Instance.Open<CanvasShop>().OnInit(GameManager.GetPlayerColor());
    }
    // ham khoi tao cac button level
    private void SetupButtonLevels()
    {
        if (levels == null || levels.Count == 0)
        {
            Debug.LogError("No button level assigned to UI");
            return;
        }
        for (int i = 0; i < levels.Count; i++)
        {
            levels[i].OnInit(this,DataManager.IsLevelUnlock(i+1));
        }
        DeactivateAllFocus();
        SetDefaultLevelStage();
        for (int i = 0; i < levels.Count; i++)
        {
            if (i == LevelManager.GetCurrentLevel() - 1)
            {
                levels[i].SetActiveFocus(true);
                levels[i].SetSelectStage();
            }
        }
    }
    // ham tat focus tat ca level button
    public void DeactivateAllFocus()
    {
        if (levels == null || levels.Count == 0)
        {
            Debug.LogError("No levels assigned to CanvasMainMenu");
            return;
        }
        for (int i = 0; i < levels.Count; i++)
        {
            levels[i].SetActiveFocus(false);
        }
    }
    // ham set stage cho button level
    public void SetDefaultLevelStage()
    {
        if (levels == null || levels.Count == 0)
        {
            Debug.LogError("No levels assigned to CanvasMainMenu");
            return;
        }
        for (int i = 0; i < levels.Count; i++)
        {
            int levelID = i + 1;
            levels[i].SetDefaulStage(DataManager.IsLevelUnlock(levelID));
            levels[i].ActivateStars(DataManager.GetStarLevel(levelID));
        }
    }
}
