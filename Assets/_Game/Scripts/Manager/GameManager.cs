using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using MyNamespace;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private CameraFollow camera;
    [SerializeField] private GameState gameState = GameState.Playing;
    
    public GameState GameState{ get { return gameState; } }
    private LevelManager LevelManager => LevelManager.Instance;
    private UIController UIController => UIController.Instance;
    private DataManager DataManager => DataManager.Instance;
    private InputManager InputManager => InputManager.Instance;
    public static Action EndGameAction;

    private void Start()
    {
        OnInit();
    }

    private void OnInit()
    {
        gameState = GameState.OnMain;
        UIController.ShowMenu();
        LevelManager.OnInit();
    }

    private void OnDespawn()
    {
    }
    
    public void OnEndGame()
    {
        this.gameState = GameState.EndGame;
        camera.SetEndGame(true);
        StartCoroutine(CoEndGameHandler());
    }

    private IEnumerator CoEndGameHandler()
    {
        EndGameAction?.Invoke();
        yield return UIController.ShowEndGameMenu(LevelManager.IsWinGame());
        InputManager.Despawn();
        LevelManager.OnEndGame();
    }
  
   public void NextLevel()
    {
        Debug.Log("Play Game");
        LevelManager.OnNext();
        camera.SetEndCamTF(LevelManager.GetEndCamTF());
        camera.SetEndGame(false);
        UIController.ShowGamePlay();
        UIController.ShowLoading();
    }

    public void ChangeState(GameState newState)
    {
        if (newState == this.gameState) return;
        this.gameState = newState;
    }
    
    // ham goi khi nguoi choi bam vao nut play
    public void PlayGame()
    {
        LevelManager.OnPlayGame();
        camera.SetEndCamTF(LevelManager.GetEndCamTF());
        camera.SetEndGame(false);
        Debug.Log("Play Game");
        UIController.ShowGamePlay();
        UIController.ShowLoading();
    }
    // ham goi khi game duoc restart ( thanh game moi)
    public void NewGame()
    {
        LevelManager.OnNewGame();
        Debug.Log("New Game");
        camera.SetEndCamTF(LevelManager.GetEndCamTF());
        camera.SetEndGame(false);
        UIController.ShowGamePlay();
        UIController.ShowLoading();
    }

    public void OnMainMenu()
    {
        this.gameState = GameState.OnMain;
        LevelManager.Despawn();
    }

    public void LoadingComplete()
    {
        InputManager.OnInit();
        ChangeState(GameState.Playing);
    }
}
