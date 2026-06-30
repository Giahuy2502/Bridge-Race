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
    private Dictionary<ColorType, List<Vector3>> emptyPositions = new Dictionary<ColorType, List<Vector3>>();
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
        if (other.CompareTag(Variables.BOT_TAG)|| other.CompareTag(Variables.PLAYER_TAG))
        {
            Character character = MyCache.GetCharacter<Character>(other);
            character.Stage = this;
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
                        Brick newBrick = SpawnBrick(hit.point,(ColorType)colorIndex);
                        bricks.Add(newBrick);
                    }
                }
            }
        }
    }

    public Brick SpawnBrick(Vector3 position, ColorType color)
    {
        Brick brick = SimplePool.Spawn<Brick>(PoolType.Brick, position, Quaternion.identity);
        brick.OnInit(color, this);
        brick.SetStartPosition(position);
        activeBricks.Add(brick);
        return brick;
    }

    public void RespawnBrick(ColorType color)
    {
        foreach(Brick brick in bricks)
        {
            if (!emptyPositions.ContainsKey(color))
            {
                Debug.Log("empty position not contained color: "+ color);
                return;
            }
            if (brick.ColorType == color && emptyPositions[color].Contains(brick.StartPosition))
            {
                brick.TF.SetParent(null);
                brick.TF.position = brick.StartPosition;
                brick.TF.rotation = Quaternion.identity;
                brick.gameObject.SetActive(true);
                brick.OnInit(color, this); 
                activeBricks.Add(brick);
                emptyPositions[color].Remove(brick.StartPosition);
                break;
            }
        }
    }

    public void Despawn(Brick brick)
    
    {
        if (!activeBricks.Contains(brick) || !bricks.Contains(brick)) return;
        activeBricks.Remove(brick);
        if (!emptyPositions.ContainsKey(brick.ColorType))
        {
            emptyPositions.Add(brick.ColorType, new List<Vector3>());
        }
        if (!emptyPositions[brick.ColorType].Contains(brick.StartPosition))
        {
            emptyPositions[brick.ColorType].Add(brick.StartPosition);
        }
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
            if (stair.ColorType != color)
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
        }

        if (stairWalkable >= bridge.Stairs.Count)
        {
            stairWalkable = bridge.Stairs.Count;
        }
        return stairWalkable;
    }
}
