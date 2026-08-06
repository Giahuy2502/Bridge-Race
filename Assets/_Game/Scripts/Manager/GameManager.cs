using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using MyNamespace;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private CameraFollow camera;
    [SerializeField] private GameState gameState = GameState.Playing;
    [SerializeField] private ColorType playerColor;
    private LevelManager LevelManager => LevelManager.Instance;
    private UIController UIController => UIController.Instance;
    private SoundManager SoundManager => SoundManager.Instance;
    private InputManager InputManager => InputManager.Instance;
    private DataManager DataManager => DataManager.Instance;
    private RankManager RankManager => RankManager.Instance;
    
    private void Awake()
    {
        Application.targetFrameRate = 60;
        DataManager.GetPlayerData().LoadData();
    }
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
    // ham duoc goi khi endgame
    public void OnEndGame()
    {
        this.gameState = GameState.EndGame;
        StartCoroutine(CoEndGameHandler());
    }

    public void SetEndCam()
    {
        camera.SetEndGame(true);
    }
    private IEnumerator CoEndGameHandler()
    {
        RankManager.SortCharacters();
        yield return UIController.ShowEndGameMenu(LevelManager.IsWinGame());
        InputManager.Despawn();
        LevelManager.OnEndGame();
    }
    // ham duoc goi khi chuyen sang level tiep theo
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
    // ham duoc goi moi khi quay lai main menu
    public void OnMainMenu()
    {
        this.gameState = GameState.OnMain;
        LevelManager.Despawn();
        SoundManager.ChangeSound(SoundID.BG_MainMenu,0f);
    }
    // ham duoc goi khi loading canvas chay xong
    public void LoadingComplete()
    {
        UIController.PlayCountdown();
        SoundManager.ChangeSound(SoundID.BG_GamePlay,0f);
    }
    // ham chuyen trang thai game khi countdown chay xong
    public void ChangeStateOnCountDown()
    {
        if (gameState != GameState.Pause)
        {
            ChangeState(GameState.Playing);
        }
    }
    // luu data khi thoat game
    private void OnApplicationQuit()
    {
        DataManager.GetPlayerData().SaveData();
    }
    // luu data khi thoat game
    public void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            DataManager.GetPlayerData().SaveData();
        }
    }

    public void SetUpJoyStick()
    {
        UIController.ShowJoyStick();
        InputManager.OnInit();
    }

    public GameState GetGameState()
    {
        return gameState;
    }

    public ColorType GetPlayerColor()
    {
        return playerColor;
    }

    public void SetPlayerColor(ColorType playerColor)
    {
        this.playerColor = playerColor;
    }
}
