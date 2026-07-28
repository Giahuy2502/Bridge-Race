using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using MyNamespace;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : Singleton<GameManager>
{
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
    
    public void OnWinGame()
    {
        this.gameState = GameState.EndGame;
        StartCoroutine(CoEndGameHandler());

    }

    private IEnumerator CoEndGameHandler()
    {
        EndGameAction?.Invoke();
        yield return UIController.ShowEndGameMenu(LevelManager.IsWinGame());
        InputManager.Despawn();
        LevelManager.OnEndGame();
    }
    public void OnLoseGame()
    {
    }

    public void Restart()
    {
        LevelManager.OnPlayGame();
        Debug.Log("Play Game");
    }

    public void NextLevel()
    {
        Debug.Log("Play Game");
        LevelManager.OnNext();
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
        // OnDespawn();
        // OnInit();
        LevelManager.OnPlayGame();
        Debug.Log("Play Game");
        UIController.ShowGamePlay();
        UIController.ShowLoading();
    }
    // ham goi khi game duoc restart ( thanh game moi)
    public void NewGame()
    {
        OnDespawn();
        OnInit();
        LevelManager.OnPlayGame();
        Debug.Log("New Game");
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
