using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasMainMenu : UICanvas
{
    public void PlayButton()
    {
        Close(0);
        UIManager.Instance.Open<CanvasGamePlay>();
    }

    public void SettingButton()
    {
        UIManager.Instance.Open<CanvasSettings>().SetState(this);
    }
}
