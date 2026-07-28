using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanvasFail : UICanvas
{
    private GameManager GameManager => GameManager.Instance;
     public void MainMenuButton()
     {
            GameManager.OnMainMenu();
            Close(0);
            UIManager.Instance.Open<CanvasMainMenu>();
     }

     public void RetryButton()
     {
         GameManager.OnMainMenu();
         Close(0);
         UIManager.Instance.Open<CanvasMainMenu>();
     }
}
