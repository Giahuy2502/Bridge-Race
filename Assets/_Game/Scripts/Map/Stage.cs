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
    [SerializeField] private Transform bricksParent;
    [SerializeField] private float step;
    [SerializeField] private float offset = 2f;
    [SerializeField] private List<ColorType> colors = new List<ColorType>();
    private List<Brick> activeBricks = new List<Brick>();
    private List<ColorType> activeColors = new List<ColorType>();
    private Dictionary<ColorType, List<Vector3>> emptyPositions = new Dictionary<ColorType, List<Vector3>>();
    

    private void Start()
    {
        OnInit();
    }

    private void OnInit()
    {
        activeColors.Clear();
        GenerateBricks();
        DeactiveAllBricks();
        SetOnInitBridge(this);
    }

    

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Variables.BOT_TAG)|| other.CompareTag(Variables.PLAYER_TAG))
        {
            Character character = MyCache.GetCharacter<Character>(other);
            character.Stage = this;
            ActiveColorBricks(character.ColorType);
        }
    }
    private void GenerateBricks()
    {
        bricks.Clear();
        Bounds stageBounds = stageCollider.bounds;
        List<Vector3> spawnPoints = new List<Vector3>();
        for (float x = stageBounds.min.x + offset; x <= stageBounds.max.x - offset ; x += step)
        {
            for (float z = stageBounds.min.z + offset; z <= stageBounds.max.z + offset ; z += step)
            {
                Vector3 rayOrigin = new Vector3(x, stageBounds.max.y + 1f, z); 
                if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, stageBounds.size.y + 2f))
                {
                    if (hit.collider == stageCollider)
                    {
                        spawnPoints.Add(hit.point+Vector3.up*0.125f);
                    }
                }
            }
        }
        
        int totalBricks = spawnPoints.Count;
        int totalColors = colors.Count;
        int bricksPerColor = totalBricks / totalColors;
        int extraBricks = totalBricks % totalColors;
        
        List<ColorType> colorGenerates = new List<ColorType>();
        for (int i = 0; i < totalColors; i++)
        {
            int bricksCount = bricksPerColor;
            if (i < extraBricks)
            {
                bricksCount++;
            }
            for (int j = 0; j < bricksCount; j++)
            {
                colorGenerates.Add(colors[i]);
            }
        }
        for (int i = 0; i < totalBricks; i++)
        {
            ColorType temp = colorGenerates[i];
            int randomIndex = Random.Range(i, totalBricks);
            colorGenerates[i] = colorGenerates[randomIndex];
            colorGenerates[randomIndex] = temp;
        }
        for (int i = 0; i < totalBricks; i++)
        {
            Brick newBrick = SpawnBrick(spawnPoints[i], colorGenerates[i]);
            bricks.Add(newBrick);
        }
    }
    private Brick SpawnBrick(Vector3 position, ColorType color)
    {
        Brick brick = SimplePool.Spawn<Brick>(PoolType.Brick, position, Quaternion.identity);
        brick.OnInit(color, this);
        brick.SetStartPosition(position);
        brick.gameObject.SetActive(false);
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
                brick.TF.SetParent(bricksParent);
                brick.TF.position = brick.StartPosition;
                brick.TF.rotation = Quaternion.identity;
                brick.OnInit(color, this); 
                brick.gameObject.SetActive(true);
                activeBricks.Add(brick);
                emptyPositions[color].Remove(brick.StartPosition);
                break;
            }
        }
    }
    public void DespawnBrick(Brick brick)
    {
        brick.TF.SetParent(bricksParent);
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
    private void Despawn()
    {
        
    }

    private void ActiveColorBricks(ColorType color)
    {
        if (activeColors.Contains(color)) return;
        activeColors.Add(color);
        foreach (Brick brick in bricks)
        {
            if (brick.ColorType == color)
            {
                brick.gameObject.SetActive(true);
                if (!activeBricks.Contains(brick))
                {
                    activeBricks.Add(brick);
                }
            }
        }
    }
    private void DeactiveAllBricks()
    {
        foreach (Brick brick in bricks)
        {
            brick.gameObject.SetActive(false);
        }
    }
    private void SetOnInitBridge(Stage stage)
    {
        foreach (Bridge bridge in bridges)
        {
            bridge.OnInit(stage);
        }
    }

}
