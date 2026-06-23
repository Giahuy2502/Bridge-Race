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
        if (other.CompareTag(Variables.BOT_TAG))
        {
            Bot bot = MyCache.GetCharacter<Bot>(other);
            bot.Stage = this;
        }
    }

    public void GenerateBricks()
    {
        bricks.Clear();
        Bounds stageBounds = stageCollider.bounds;
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

    public Vector3 GetNearestBrick(Bot bot)
    {
        bool hasBrickSameColor = false;
        ColorType color = bot.ColorType;
        Vector3 nearestBrick = Vector3.positiveInfinity;
        foreach (Brick brick in bricks)
        {
            if (brick.ColorType == color)
            {
                hasBrickSameColor = true;
                float distance = Vector3.Distance(bot.transform.position, brick.transform.position);
                if (distance <= Vector3.Distance(nearestBrick, bot.transform.position))
                {
                    nearestBrick = brick.transform.position;
                }
            }
        }
        if(!hasBrickSameColor) return bot.transform.position;
        return nearestBrick;
    }
}
