using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    
    [SerializeField] private MapDataSO mapDataSO;
    [SerializeField] private PlayerData playerData;
    private Level level;
    
    public Level Level { get => level;private set => level = value; }
    public void LoadLevel(int levelIndex)
    {
        DespawnCurrentLevel();
        Level mapData = mapDataSO.GetMapObject(levelIndex);
        level = Instantiate(mapData);
        if (level != null)
        {
            level.OnInit();
            Debug.Log("level loaded");
        }
    }

    public void ResetData()
    {
        level = null;
        playerData.ResetData();
    }
    public void DespawnCurrentLevel()
    {
        if (level == null) return;
        level.Despawn();
        Destroy(level.gameObject);
        level = null;
    }
    public int GetNextLevel(int prevlevel)
    {
        int newLevel = prevlevel + 1;
        if (newLevel >= mapDataSO.GetMapObjectCount())
        {
            newLevel = mapDataSO.GetMapObjectCount();
        }
        return newLevel;
    }
    public void UnlockLevel(int levelID)
    {
        if (levelID > playerData.PlayerLevelData.Count)
        {
            Debug.LogError("Player level id is out of range");
            return;
        }
        playerData.UnlockLevel(levelID);
    }

    public void SetStartLevel(int levelID, int star)
    {
        playerData.SetStar(levelID, star);
    }

    public bool IsLevelUnlock(int levelID)
    {
        return playerData.IsLevelUnlocked(levelID);
    }

    public int GetStarLevel(int levelID)
    {
        return playerData.GetStarLevel(levelID);
    }
}
