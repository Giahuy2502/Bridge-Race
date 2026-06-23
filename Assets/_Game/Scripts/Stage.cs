using System;
using System.Collections;
using System.Collections.Generic;
using MyNamespace;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;
using Variables = MyNamespace.Variables;

public class Stage : MonoBehaviour
{
    [SerializeField] private List<Brick> bricks = new List<Brick>();
    [SerializeField] private Collider stageCollider;
    [SerializeField] private GameObject brickPrefab;
    [SerializeField] private float step;
    private float offSetX;
    private float offSetZ;
    

    private void Start()
    {
        offSetX = brickPrefab.transform.localScale.x;
        offSetZ = brickPrefab.transform.localScale.z;
        GenerateBricks();
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Variables.PLAYER_TAG))
        {
            
        }
    }

    public void GenerateBricks()
    {
        bricks.Clear();
        Bounds stageBounds = stageCollider.bounds;
        Debug.Log("Stage Bounds: "+ stageBounds.min.x +" "+ stageBounds.min.z +" "+ stageBounds.max.x +" "+ stageBounds.max.z);
        for (float x = stageBounds.min.x + offSetX; x <= stageBounds.max.x -offSetX ; x += step)
        {
            for (float z = stageBounds.min.z + offSetZ; z <= stageBounds.max.z + offSetZ ; z += step)
            {
                Vector3 rayOrigin = new Vector3(x, stageBounds.max.y + 1f, z); 
                if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, stageBounds.size.y + 2f))
                {
                    if (hit.collider == stageCollider)
                    {
                        int colorIndex = Random.Range(1, 7);
                        SpawnBrick(hit.point,(ColorType)colorIndex);
                    }
                }
            }
        }
    }

    public void SpawnBrick(Vector3 position, ColorType color)
    {
        Brick brick = SimplePool.Spawn<Brick>(PoolType.Brick, position, Quaternion.identity);
        brick.OnInit(color);
        brick.Stage = this;
        bricks.Add(brick);
    }

    public void Despawn(Brick brick)
    {
        if (!bricks.Contains(brick)) return;
        bricks.Remove(brick);
    }
}
