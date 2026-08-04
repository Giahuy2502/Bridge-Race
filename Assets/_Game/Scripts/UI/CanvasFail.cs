using System.Collections;
using System.Collections.Generic;
using MyNamespace;
using TMPro;
using UnityEngine;

public class CanvasFail : UICanvas
{
    private GameManager GameManager => GameManager.Instance;
    private SoundManager SoundManager => SoundManager.Instance;
    public override void Setup()
    {
        base.Setup();
        SoundManager.PlayFx(FxID.SFX_Win);
        SoundManager.ChangeSound(SoundID.BG_MainMenu,1f);
    }
    // quay lai menu
     public void MainMenuButton()
     {
            GameManager.OnMainMenu();
            Close(0);
            UIManager.Instance.Open<CanvasMainMenu>();
     }
     // choi lai level
     public void RetryButton()
     {
         GameManager.PlayGame();
         Close(0);
     }
}
