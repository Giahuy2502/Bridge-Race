using System.Collections;
using System.Collections.Generic;
using MyNamespace;
using UnityEngine;

public class CanvasSettings : UICanvas
{
    [SerializeField] private GameObject[] buttons;
    [SerializeField] private SoundButton soundButton;
    [SerializeField] private SFXButton sfxButton;
    private UICanvas prevCanvas;
    private GameManager GameManager => GameManager.Instance;
    private LevelManager LevelManager => LevelManager.Instance;

    public override void Setup()
    {
        base.Setup();
        soundButton.OnInit();
        sfxButton.OnInit();
    }
    public void SetState(UICanvas canvas)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].gameObject.SetActive(false);
        }
        if (canvas is CanvasMainMenu)
        {
            buttons[0].gameObject.SetActive(true);
            buttons[4].gameObject.SetActive(true);
            buttons[5].gameObject.SetActive(true);
        }
        else if (canvas is CanvasGamePlay)
        {
            buttons[1].gameObject.SetActive(true);
            buttons[2].gameObject.SetActive(true);
            buttons[3].gameObject.SetActive(true);
            buttons[4].gameObject.SetActive(true);
            buttons[5].gameObject.SetActive(true);
        }
        this.prevCanvas = canvas;
    }
    // ham quay lai main menu
    public void MainMenuButton()
    {
        this.prevCanvas = null;
        UIManager.Instance.ClodeAll();
        GameManager.OnMainMenu();
        UIManager.Instance.Open<CanvasMainMenu>();
    }
    // ham choi lai man choi hien tai
    public void RetryButton()
    {
        this.prevCanvas = null;
        GameManager.PlayGame();
        Close(0);
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
