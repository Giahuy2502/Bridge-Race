using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasMainMenu : UICanvas
{
    private GameManager GameManager => GameManager.Instance;
    private UIController UIController => UIController.Instance;
    public void PlayButton()
    {
        GameManager.PlayGame();
        Close(0);
        UIController.ShowGamePlay();
    }

    public void SettingButton()
    {
        UIManager.Instance.Open<CanvasSettings>().SetState(this);
    }
}
