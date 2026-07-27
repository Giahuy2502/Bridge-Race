using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasMainMenu : UICanvas
{
    private GameManager GameManager => GameManager.Instance;
    public void PlayButton()
    {
        GameManager.PlayGame();
        Close(0);
    }

    public void SettingButton()
    {
        UIManager.Instance.Open<CanvasSettings>().SetState(this);
    }
}
