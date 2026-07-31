using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private NavMeshSurface surface;
    [SerializeField] private List<Platform> platforms = new List<Platform>();
    [SerializeField] private FinishPlatform finishPlatform;
    [SerializeField] private Transform endCamTF;
    
    public void OnInit()
    {
        if (platforms == null || platforms.Count == 0)
        {
            Debug.LogError("No platforms assigned!");
            return;
        }

        foreach (Platform platform in platforms)
        {
            platform.OnInit();
        }

        if (finishPlatform == null)
        {
            Debug.LogError("No finish platform assigned!");
        }
        finishPlatform.OnInit();
        
        // sau khi spawn xong cac platform thi phai bake lai surface
        // if (surface == null)
        // {
        //     Debug.LogError("No surface assigned!");
        //     return;
        // }
        // surface.BuildNavMesh();
    }

    public void Despawn()
    {
        if (platforms == null || platforms.Count == 0)
        {
            Debug.LogError("No platforms assigned!");
            return;
        }

        foreach (Platform platform in platforms)
        {
            platform.Despawn();
        }
        Destroy(gameObject);
    }

    public Transform GetEndCameraTF()
    {
        return endCamTF;
    }
}
