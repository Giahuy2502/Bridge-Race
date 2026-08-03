using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    
    [SerializeField] private MapDataSO mapDataSO;
    [SerializeField] private PlayerData playerData;
    private Level currentLevel;
    
    // ham duoc goi khi khoi tao level
    public void LoadLevel(int levelIndex)
    {
        DespawnCurrentLevel();
        Level mapData = mapDataSO.GetMapObject(levelIndex);
        currentLevel = Instantiate(mapData);
        if (currentLevel != null)
        {
            currentLevel.OnInit();
            Debug.Log("level loaded");
        }
    }

    // ham duoc goi khi reset du lieu thanh game moi
    public void ResetData()
    {
        currentLevel = null;
        playerData.ResetData();
    }
    
    // ham duoc goi de destroy map hien tai
    public void DespawnCurrentLevel()
    {
        if (currentLevel == null) return;
        currentLevel.Despawn();
        Destroy(currentLevel.gameObject);
        currentLevel = null;
    }
    // ham lay level tiep theo
    public int GetNextLevel(int prevlevel)
    {
        int newLevel = prevlevel + 1;
        if (newLevel >= mapDataSO.GetMapObjectCount())
        {
            newLevel = mapDataSO.GetMapObjectCount();
        }
        return newLevel;
    }
    // ham mo khoa level moi
    public void UnlockLevel(int levelID)
    {
        if (levelID > playerData.PlayerLevelData.Count)
        {
           return;
        }
        playerData.UnlockLevel(levelID);
    }
    // ham set star cho level
    public void SetStartLevel(int levelID, int star)
    {
        playerData.SetStar(levelID, star);
    }
    // ham kiem tra xem level duoc unlock chua
    public bool IsLevelUnlock(int levelID)
    {
        return playerData.IsLevelUnlocked(levelID);
    }
    // ham lay star cua 1 level
    public int GetStarLevel(int levelID)
    {
        return playerData.GetStarLevel(levelID);
    }
    // ham kiem tra xem sound on 
    public bool GetIsSoundOn()
    {
        return playerData.ISSoundOn;
    }
    // ham kiem tra xem music on 
    public bool GetIsMusicOn()
    {
        return playerData.ISSFXOn;
    }
    public PlayerData GetPlayerData()
    {
        return playerData;
    }
    public Level GetCurrentLevel()
    {
        return currentLevel;
    }
}
