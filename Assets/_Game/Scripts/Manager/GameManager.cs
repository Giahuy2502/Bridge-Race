using System;
using System.Collections;
using System.Collections.Generic;
using MyNamespace;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private GameState gameState = GameState.Playing;
    
    public GameState GameState{ get { return gameState; } }
    private LevelManager LevelManager => LevelManager.Instance;
    public static Action WinAction;

    private void Start()
    {
        OnInit();
    }

    private void OnInit()
    {
        gameState = GameState.Playing;
        LevelManager.OnInit();
    }

    private void OnDespawn()
    {
    }
    
    public void OnWinGame()
    {
        this.gameState = GameState.Win;
        WinAction?.Invoke();
    }
    
    public void OnLoseGame()
    {
    }

    public void Restart()
    {
    }

    public void NextLevel()
    {
    }

    public void ChangeState()
    {
    }

    public void NewGame()
    {
    }
}
