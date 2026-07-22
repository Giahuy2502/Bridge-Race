using System.Collections;
using System.Collections.Generic;
using MyNamespace;
using UnityEngine;

public class CanvasSettings : UICanvas
{
    [SerializeField] private GameObject[] buttons;
    private UICanvas prevCanvas;
    private GameManager GameManager => GameManager.Instance;
    private LevelManager LevelManager => LevelManager.Instance;

    public void SetState(UICanvas canvas)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].gameObject.SetActive(false);
        }
        if (canvas is CanvasMainMenu)
        {
            buttons[2].gameObject.SetActive(true);
        }
        else if (canvas is CanvasGamePlay)
        {
            buttons[0].gameObject.SetActive(true);
            buttons[1].gameObject.SetActive(true);
        }
        this.prevCanvas = canvas;
    }
    public void MainMenuButton()
    {
        UIManager.Instance.ClodeAll();
        GameManager.OnMainMenu();
        UIManager.Instance.Open<CanvasMainMenu>();
    }

    public override void Open()
    {
        LevelManager.OnPause();
        base.Open();
    }

    public override void CloseDirectly()
    {
        if (prevCanvas is CanvasMainMenu)
        {
            LevelManager.OnContinue(GameState.OnMain);
        }
        else if (prevCanvas is CanvasGamePlay)
        {
            LevelManager.OnContinue(GameState.Playing);
        }
        this.prevCanvas = null;
        base.CloseDirectly();
    }
}
