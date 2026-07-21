using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    // [SerializeField] private List<Level> levels;
    [SerializeField] private MapDataSO mapDataSO;
    [SerializeField] private Level level;
    public void LoadLevel(int levelIndex)
    {
        DespawnCurrentLevel();
        // if (levels == null || levels.Count == 0 || levelIndex < 0 || levelIndex >= levels.Count)
        // {
        //     Debug.LogError("Invalid level index");
        //     return;
        // }
        Level mapData = mapDataSO.GetMapObject(levelIndex);
        level = Instantiate(mapData);
        if (level != null)
        {
            level.OnInit();
            Debug.Log("level loaded");
        }
    }

    public void DespawnLevel(int levelIndex)
    {
        // if (levels == null || levels.Count == 0 || levelIndex < 0 || levelIndex >= levels.Count)
        // {
        //     Debug.LogError("Invalid level index");
        //     return;
        // }
        // Level currentLevel = levels[levelIndex];
        if (level == null) return;
        level.Despawn();
    }
    public void DespawnCurrentLevel()
    {
        if (level == null) return;
        level.Despawn();
        Destroy(level.gameObject);
        level = null;
    }
}
