using System;
using System.Collections;
using System.Collections.Generic;
using MyNamespace;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private List<CharacterData> characterDatas = new List<CharacterData>();
    [SerializeField] private List<Transform> startPoints;
    [SerializeField] private Transform brickParentTF;
    [SerializeField] private int currentLevel = 0;
    private RankManager RankManager => RankManager.Instance;
    private GameManager GameManager => GameManager.Instance;
    private DataManager DataManager => DataManager.Instance;
    public void OnInit()
    {
        RankManager.OnInit();
    }
    // ham duoc goi de khoi tao level
    private void LoadLevel(int level)
    {
        SetCharacterColor(GameManager.GetPlayerColor());
        DataManager.LoadLevel(level);
        StartCoroutine(ISpawnCharactersRoutine());
    }

    private void OnPlay()
    {
        // GameManager.ChangeState(GameState.Playing);
    }
    // ham duoc goi khi muon pause game
    public void OnPause()
    {
        GameManager.ChangeState(GameState.Pause);
    }
    // ham duoc goi khi muon continue game
    public void OnContinue(GameState newState)
    {
        GameManager.ChangeState(newState);
    }

    public void Despawn()
    {
        DataManager.DespawnCurrentLevel();
        DespawnCharacters();
    }
    // ham duoc goi khi ket thuc game
    public void OnEndGame()
    {
        Despawn();
    }
    // ham duoc goi khi muon reset thanh game moi
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
    // ham duoc goi khi khoi tao nhan vat
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
    // ham duoc goi khi muon despawn nhan vat
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
    // ham lay character data cua nguoi choi
    public CharacterData GetPlayerData()
    {
        return characterDatas[0];
    }
    // ham set color cho character voi tham so la player color
    private void SetCharacterColor(ColorType playerColor)
    {
        if (characterDatas == null || characterDatas.Count == 0)
        {
            Debug.LogError("No characters assigned");
            return;
        }
        CharacterData playerData = GetPlayerData();
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
    // ham kiem tra xem color moi nay da duoc character khac su dung chua
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
    // ham kiem tra xem player thang hay thua
    public bool IsWinGame()
    {
        return !RankManager.IsPlayerLose();
    }
    // ham set next level
    public void SetNextLevel()
    {
        this.currentLevel = DataManager.GetNextLevel(currentLevel);
    }
    // ham lay endgame cam Transform cua tung level
    public Transform GetEndCamTF()
    {
        return DataManager.GetCurrentLevel().GetEndCameraTF();
    }

    public Transform GetBrickParentTF()
    {
        return brickParentTF;
    }

    public int GetCurrentLevel()
    {
        return currentLevel;
    }

    public void SetCurrentLevel(int newLevel)
    {
        currentLevel = newLevel;
    }
}

[Serializable]
public class CharacterData
{
    public Character Character;
    public ColorType Color;
}
