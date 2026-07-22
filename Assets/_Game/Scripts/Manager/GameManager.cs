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
    public static Action WinAction;

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
        this.gameState = GameState.Win;
        UIController.ShowWinMenu();
        LevelManager.Despawn();
        WinAction?.Invoke();
    }
    
    public void OnLoseGame()
    {
    }

    public void Restart()
    {
        LevelManager.OnRestart();
        Debug.Log("Play Game");
    }

    public void NextLevel()
    {
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
        LevelManager.OnRestart();
        Debug.Log("Play Game");
    }
    // ham goi khi game duoc restart ( thanh game moi)
    public void NewGame()
    {
        OnDespawn();
        OnInit();
        LevelManager.OnRestart();
        Debug.Log("New Game");
    }

    public void OnMainMenu()
    {
        this.gameState = GameState.OnMain;
        LevelManager.Despawn();
    }
}
