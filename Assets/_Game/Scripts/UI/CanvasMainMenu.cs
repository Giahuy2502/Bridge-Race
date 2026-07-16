using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasMainMenu : UICanvas
{
    private GameManager GameManager => GameManager.Instance;
    public void PlayButton()
    {
        GameManager.NewGame();
        Close(0);
        UIManager.Instance.Open<CanvasGamePlay>();
    }

    public void SettingButton()
    {
        UIManager.Instance.Open<CanvasSettings>().SetState(this);
    }
}
