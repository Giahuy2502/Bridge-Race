using System.Collections;
using System.Collections.Generic;
using MyNamespace;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private List<Character> characters = new List<Character>();
    private RankManager RankManager => RankManager.Instance;
    private GameManager GameManager => GameManager.Instance;
    
    private DataManager DataManager => DataManager.Instance;
    public List<Character> Characters { get => characters; set => characters = value; }
    public void OnInit()
    {
        RankManager.OnInit();
        // GameManager.WinAction += OnPause;
    }

    public void LoadLevel()
    {
        // data manager load map
        DataManager.LoadLevel();
        // truyen character vao cac class can su dung
        
    }

    public void OnPlay()
    {
        GameManager.ChangeState(GameState.Playing);
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
    // ham goi khi choi lai level hien tai

    public void OnRestart()
    {
        OnDespawn();
        LoadLevel();
        OnInit();
        OnPlay();
    }
    // ham goi khi chuyen sang level tiep theo
    public void OnNext()
    {
        OnDespawn();
        LoadLevel();
        OnInit();
        OnPlay();
    }
    
    
}
