using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasMainMenu : UICanvas
{
    [SerializeField] List<ButtonLevel> levels = new List<ButtonLevel>();
    private GameManager GameManager => GameManager.Instance;
    private LevelManager LevelManager => LevelManager.Instance;

    public override void Setup()
    {
        base.Setup();
        SetupButtonLevels();
    }

    public void PlayButton()
    {
        GameManager.PlayGame();
        Close(0);
    }

    public void SettingButton()
    {
        UIManager.Instance.Open<CanvasSettings>().SetState(this);
    }

    public void SetupButtonLevels()
    {
        if (levels == null || levels.Count == 0)
        {
            Debug.LogError("No button level assigned to UI");
            return;
        }
        for (int i = 0; i < levels.Count; i++)
        {
            levels[i].OnInit(this);
        }
        DeactivateAllFocus();
        for (int i = 0; i < levels.Count; i++)
        {
            if (i == LevelManager.CurrentLevel - 1)
            {
                levels[i].SetActiveFocus(true);
            }
        }
    }

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
}
