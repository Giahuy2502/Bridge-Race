using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    // [SerializeField] private List<Level> levels;
    [SerializeField] private MapDataSO mapDataSO;
    private Level level;
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
}
