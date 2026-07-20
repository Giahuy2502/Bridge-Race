using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    [SerializeField] private List<Level> levels;
    public void LoadLevel(int levelIndex)
    {
        if (levels == null || levels.Count == 0 || levelIndex < 0 || levelIndex >= levels.Count)
        {
            Debug.LogError("Invalid level index");
            return;
        }
        Level currentLevel = levels[levelIndex];
        currentLevel.OnInit();
    }

    public void DespawnLevel(int levelIndex)
    {
        if (levels == null || levels.Count == 0 || levelIndex < 0 || levelIndex >= levels.Count)
        {
            Debug.LogError("Invalid level index");
            return;
        }
        Level currentLevel = levels[levelIndex];
        currentLevel.Despawn();
    }
}
