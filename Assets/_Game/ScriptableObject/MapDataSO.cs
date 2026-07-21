using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "MapDataSO", menuName = "MapDataSO", order = 2)]
public class MapDataSO : ScriptableObject
{
    [SerializeField] private List<Level> mapObjects;

    public Level GetMapObject(int index)
    {
        if (index < 0 || index >= mapObjects.Count)
        {
            Debug.LogError($"Invalid index {index}");
        }
        return mapObjects[index];
    }
}

// [Serializable]
// public class MapData
// {
//     [SerializeField] private GameObject mapObjects;
//     [SerializeField] private Level level;
//     
//     public GameObject MapObjects{ get => mapObjects; set => mapObjects = value; }
//     public Level Level { get => level; set => level = value; }
// }
