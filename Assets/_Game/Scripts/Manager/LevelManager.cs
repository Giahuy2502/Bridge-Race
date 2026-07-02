using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private List<Character> characters = new List<Character>();
    private RankManager RankManager => RankManager.Instance;
    private GameManager GameManager => GameManager.Instance;
    public List<Character> Characters { get => characters; set => characters = value; }
    public void OnInit()
    {
        RankManager.OnInit();
        // GameManager.WinAction += OnPause;
    }

    public void LoadLevel()
    {
    }

    public void OnPlay()
    {
        
    }

    public void OnPause()
    {
        Time.timeScale = 0f;
    }

    public void OnContinue()
    {
        Time.timeScale = 1f;
    }

    public void OnDespawn()
    {
      
    }

    public void OnWin()
    {
        
    }

    public void OnLose()
    {
       
    }

    public void OnRestart()
    {
        OnDespawn();
        LoadLevel();
        OnInit();
        OnPlay();
    }

    public void OnNext()
    {
        OnDespawn();
        LoadLevel();
        OnInit();
        OnPlay();
    }
    
    
}
