using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MyNamespace;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;
using Variables = MyNamespace.Variables;

public class Stage : MonoBehaviour
{
    [SerializeField] private List<Brick> bricks = new List<Brick>();
    [SerializeField] private List<Bridge> bridges = new List<Bridge>();
    [SerializeField] private Collider stageCollider;
    [SerializeField] private GameObject brickPrefab;
    [SerializeField] private float step;
    private List<Brick> activeBricks = new List<Brick>();
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
        activeBricks.Add(brick);
    }

    public void Despawn(Brick brick)
    {
        if (!activeBricks.Contains(brick) || !bricks.Contains(brick)) return;
        activeBricks.Remove(brick);
    }

    // tra ve vi tri vien gach gan nhat so voi bot
    public Brick GetNearestBrick(Bot bot)
    {
        bool hasBrickSameColor = false;
        ColorType color = bot.ColorType;
        Vector3 botPosition = bot.transform.position;
        Brick nearestBrick = null;
        foreach (Brick brick in activeBricks)
        {
            if (brick.ColorType == color)
            {
                if(nearestBrick == null) nearestBrick = brick;
                hasBrickSameColor = true;
                float distance = Vector3.Distance(botPosition, brick.transform.position);
                if (distance <= Vector3.Distance(nearestBrick.transform.position, botPosition))
                {
                    nearestBrick = brick;
                }
            }
        }
        if(!hasBrickSameColor) return null;
        return nearestBrick;
    }

    // lay vi tri cay cau gan bot nhat
    public Bridge GetNearestBridge(Bot bot)
    {
        Bridge nearestBridge = bridges[0];
        foreach (Bridge bridge in bridges)
        {
            float distance = Vector3.Distance(bot.transform.position, bridge.transform.position);
            if (distance <= Vector3.Distance(nearestBridge.TF.position, bot.transform.position))
            {
                nearestBridge = bridge;
            }
        }
        return nearestBridge;
    }
    // lay so stair co the di

    public int GetStairWalkable(ColorType color, int brickCount, Bridge bridge)
    {
        int stairWalkable = 0;
        foreach (Stair stair in bridge.Stairs)
        {
            if (stair.ColorType == color)
            {
                stairWalkable++;
            }
            if (stair.ColorType == ColorType.None)
            {
                if (brickCount >= 1)
                {
                    brickCount--;
                    stairWalkable++;
                }
                else
                {
                    break;
                }
            }
            else if (stair.ColorType != color)
            {
                if (brickCount >= 2)
                {
                    brickCount -= 2;
                    stairWalkable++;
                }
                else
                {
                    break;
                }
            }
        }

        if (stairWalkable >= bridge.Stairs.Count)
        {
            stairWalkable = bridge.Stairs.Count;
        }
        return stairWalkable;
    }
}
