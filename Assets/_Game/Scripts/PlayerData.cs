using System;
using System.Collections;
using System.Collections.Generic;
using MyNamespace;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    [SerializeField] private bool isSoundOn = false;
    [SerializeField] private bool isSFXOn = false;
    [SerializeField] List<PlayerLevelData> playerLevelData;
    GameManager GameManager => GameManager.Instance;
    public bool ISSoundOn { get => isSoundOn; set => isSoundOn = value; }
    public bool ISSFXOn { get => isSFXOn; set => isSFXOn = value; }
    public List<PlayerLevelData> PlayerLevelData { get => playerLevelData; set => playerLevelData = value; }

    public void UnlockLevel(int levelID)
    {
        for(int i = 0; i < playerLevelData.Count; i++)
        {
            if (playerLevelData[i].levelID == levelID)
            {
                playerLevelData[i].Unlock();
            }
        }
    }
    public void SetStar(int levelID, int star)
    {
        for(int i = 0; i < playerLevelData.Count; i++)
        {
            if (playerLevelData[i].levelID == levelID)
            {
                playerLevelData[i].SetStars(star);
            }
        }
    }
    public void ResetData()
    {
        for(int i = 0; i < playerLevelData.Count; i++)
        {
            playerLevelData[i].Reset();
        }
        playerLevelData[0].Unlock();
    }
 
    public int GetStarLevel(int levelID)
    {
        for (int i = 0; i < playerLevelData.Count; i++)
        {
            if (playerLevelData[i].unlocked && playerLevelData[i].levelID == levelID)
            {
                return playerLevelData[i].stars;
            }
        }
        return 0;
    }

    public bool IsLevelUnlocked(int levelID)
    {
        for(int i = 0; i < playerLevelData.Count; i++)
        {
            if (playerLevelData[i].levelID == levelID)
            {
                return playerLevelData[i].unlocked;
            }
        }
        return false;
    }

    public void SaveData()
    {
        PlayerSaveData playerSaveData = new PlayerSaveData();
        playerSaveData.isSoundOn = isSoundOn;
        playerSaveData.isSFXOn = isSFXOn;
        playerSaveData.playerColor = GameManager.GetPlayerColor();
        playerSaveData.playerLevelData = playerLevelData;
        
        string json = JsonUtility.ToJson(playerSaveData);
        PlayerPrefs.SetString(Variables.SAVE_KEY, json);
        PlayerPrefs.Save();
    }

    public void LoadData()
    {
        if (PlayerPrefs.HasKey(Variables.SAVE_KEY))
        {
            string json = PlayerPrefs.GetString(Variables.SAVE_KEY);
            PlayerSaveData playerSaveData = JsonUtility.FromJson<PlayerSaveData>(json);
            isSoundOn = playerSaveData.isSoundOn;
            isSFXOn = playerSaveData.isSFXOn;
            playerLevelData = playerSaveData.playerLevelData;
            GameManager.SetPlayerColor(playerSaveData.playerColor);
        }
    }

    [ContextMenu("Save Data")]
    public void UpdateDataHandle()
    {
        SaveData();
        LoadData();
    }
    
}
[System.Serializable]
public class PlayerLevelData
{
    public int levelID;
    public bool unlocked;
    public int stars;

    public void Setup(int levelID, bool unlocked, int stars)
    {
        this.levelID = levelID;
        this.unlocked = unlocked;
        this.stars = stars;
    }

    public void Reset()
    {
        this.unlocked = false;
        this.stars = 0;
    }

    public void Unlock()
    {
        this.unlocked = true;
    }

    public void SetStars(int stars)
    {
        if (stars < this.stars)
        {
            return;
        }
        this.stars = stars;
    }
}

[Serializable]
public class PlayerSaveData
{
    public bool isSoundOn;
    public bool isSFXOn;
    public ColorType playerColor;
    public List<PlayerLevelData> playerLevelData;
}