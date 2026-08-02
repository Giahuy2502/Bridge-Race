using System;
using System.Collections;
using System.Collections.Generic;
using MyNamespace;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private List<CharacterData> characterDatas = new List<CharacterData>();
    [SerializeField] private List<Transform> startPoints;
    [SerializeField] private Transform brickParent;
    [SerializeField] private int currentLevel = 0;
    
    private bool isWinGame = false;
    private RankManager RankManager => RankManager.Instance;
    private GameManager GameManager => GameManager.Instance;
    private DataManager DataManager => DataManager.Instance;
    public Transform BrickParent { get => brickParent; set => brickParent = value; }
    public int CurrentLevel { get => currentLevel; set => currentLevel = value; }
    public void OnInit()
    {
        RankManager.OnInit();
        isWinGame = false;
        // GameManager.WinAction += OnPause;
    }

    public void LoadLevel(int level)
    {
        SetCharacterColor(GameManager.PlayerColor);
        // data manager load map
        DataManager.LoadLevel(level);
        // khoi tao character
        StartCoroutine(ISpawnCharactersRoutine());
    }

    public void OnPlay()
    {
        // GameManager.ChangeState(GameState.Playing);
    }

    public void OnPause()
    {
        GameManager.ChangeState(GameState.Pause);
    }

    public void OnContinue(GameState newState)
    {
        GameManager.ChangeState(newState);
    }

    public void Despawn()
    {
        DataManager.DespawnCurrentLevel();
        DespawnCharacters();
    }

    public void OnEndGame()
    {
        Despawn();
    }

    public void OnNewGame()
    {
        currentLevel = 1;
        DataManager.ResetData();
        Despawn();
        LoadLevel(currentLevel-1);
        OnInit();
        OnPlay();
    }
    
    // ham goi khi choi lai level hien tai

    public void OnPlayGame()
    {
        Despawn();
        LoadLevel(currentLevel-1);
        OnInit();
        OnPlay();
    }
    // ham goi khi chuyen sang level tiep theo
    public void OnNext()
    {
        Despawn();
        SetNextLevel();
        LoadLevel(currentLevel-1);
        OnInit();
        OnPlay();
    }

    private void SpawnCharacters()
    {
        if (characterDatas == null || characterDatas.Count == 0)
        {
            Debug.LogError("No characters assigned");
            return;
        }

        if (startPoints == null || startPoints.Count == 0 || startPoints.Count < characterDatas.Count)
        {
            Debug.LogError("No startPoints assigned or startPoints don't match characters");
            return;
        }

        for (int i = 0; i < characterDatas.Count; i++)
        {
            Character character = characterDatas[i].Character;
            ColorType color = characterDatas[i].Color;
            Transform startPoint = startPoints[i];
            character.OnInit(color);
            character.SetStartPoints(startPoint);
        }
    }

    IEnumerator ISpawnCharactersRoutine()
    {
        yield return new WaitForSeconds(0.25f);
        SpawnCharacters();
    }
    
    private void DespawnCharacters()
    {
        if (characterDatas == null || characterDatas.Count == 0)
        {
            Debug.LogError("No characters assigned");
            return;
        }
        
        for (int i = 0; i < characterDatas.Count; i++)
        {
            Character character = characterDatas[i].Character;
            Transform startPoint = startPoints[i];
            character.SetStartPoints(startPoint);
            character.Despawn();
        }
    }

    public List<ColorType> GetCharacterColors()
    {
        List<ColorType> colors = new List<ColorType>();
        for (int i = 0; i < characterDatas.Count; i++)
        {
            ColorType color = characterDatas[i].Color;
            colors.Add(color);
        }
        return colors;
    }

    public CharacterData GetPlayerData()
    {
        return characterDatas[0];
    }
    
    
    public void SetCharacterColor(ColorType playerColor)
    {
        if (characterDatas == null || characterDatas.Count == 0)
        {
            Debug.LogError("No characters assigned");
            return;
        }
        
        CharacterData playerData = characterDatas[0];
        playerData.Color = playerColor;

        for (int i = 1; i < characterDatas.Count; i++)
        {
            ColorType color = (ColorType)Random.Range(1, 7);
            while (!IsValidRandomColor(color,i))
            {
                color = (ColorType)Random.Range(1, 7);
            }
            characterDatas[i].Color = color;
        }
    }

    private bool IsValidRandomColor(ColorType color, int index)
    {
        for (int i = 0; i <= index; i++)
        {
            CharacterData characterData = characterDatas[i];
            if (color == characterData.Color)
            {
                return false;
            }
        }
        return true;
    }
    
    public bool IsWinGame()
    {
        return !RankManager.IsPlayerLose();
    }

    public void SetNextLevel()
    {
        this.currentLevel = DataManager.GetNextLevel(currentLevel);
    }

    public Transform GetEndCamTF()
    {
        return DataManager.Level.GetEndCameraTF();
    }
}

[Serializable]
public class CharacterData
{
    public Character Character;
    public ColorType Color;
}
