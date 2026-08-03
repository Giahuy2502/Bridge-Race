using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MyNamespace;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
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
    [SerializeField] private List<ColorType> characterColors = new List<ColorType>();
    private List<Brick> activeBricks = new List<Brick>();
    private List<ColorType> activeColors = new List<ColorType>();
    private Dictionary<ColorType, List<Vector3>> emptyPositions = new Dictionary<ColorType, List<Vector3>>(); // luu vi tri cua cac vien gach da duoc nhat
    private LevelManager LevelManager => LevelManager.Instance;
    public virtual void OnInit()
    {
        Debug.Log("Call OnInit Stage");
        activeColors.Clear();
        activeBricks.Clear();
        emptyPositions.Clear();
        bricks.Clear();
        bricksParent = LevelManager.GetBrickParentTF();
        SetCharacterColors(LevelManager.GetCharacterColors());
        StartCoroutine(IGenerateBricks());
        DeactiveAllBricks();
    }
    
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Variables.BOT_TAG)|| other.CompareTag(Variables.PLAYER_TAG))
        {
            Character character = MyCache.GetCharacter<Character>(other);
            character.SetStage(this);
            ActiveColorBricks(character.GetColorType());
        }
    }

    IEnumerator IGenerateBricks()
    {
        yield return null;
        GenerateBricks();
    }
    // ham sinh cac brick tren stage
    private void GenerateBricks()
    {
        // lay cac vi tri co the dat gach
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
        // tinh toan so luong gach tuong loai
        int totalBricks = spawnPoints.Count;
        int totalColors = characterColors.Count;
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
                colorGenerates.Add(characterColors[i]);
            }
        }
        // dao thu tu spawn cac mau gach
        for (int i = 0; i < totalBricks; i++)
        {
            ColorType temp = colorGenerates[i];
            int randomIndex = Random.Range(i, totalBricks);
            colorGenerates[i] = colorGenerates[randomIndex];
            colorGenerates[randomIndex] = temp;
        }
        // spawn gach
        for (int i = 0; i < totalBricks; i++)
        {
            Brick newBrick = SpawnBrick(spawnPoints[i], colorGenerates[i]);
            bricks.Add(newBrick);
        }
    }
    // ham spawm brick
    private Brick SpawnBrick(Vector3 position, ColorType color)
    {
        Brick brick = SimplePool.Spawn<Brick>(PoolType.Brick, position, Quaternion.identity);
        brick.OnInit(color, this);
        brick.SetStartPosition(position);
        brick.gameObject.SetActive(false);
        return brick;
    }
    // ham respawm brick khi character remove brick
    public void RespawnBrick(ColorType color)
    {
        foreach(Brick brick in bricks)
        {
            if (!emptyPositions.ContainsKey(color))
            {
                Debug.Log("empty position not contained color: "+ color);
                return;
            }
            if (brick.GetColor() == color && emptyPositions[color].Contains(brick.GetStartPosition()))
            {
                brick.TF.SetParent(bricksParent);
                brick.TF.position = brick.GetStartPosition();
                brick.TF.rotation = Quaternion.identity;
                brick.OnInit(color, this); 
                brick.gameObject.SetActive(true);
                activeBricks.Add(brick);
                emptyPositions[color].Remove(brick.GetStartPosition());
                break;
            }
        }
    }
    // ham despawn brick
    public void DespawnBrick(Brick brick)
    {
        brick.TF.SetParent(bricksParent);
        if (!activeBricks.Contains(brick) || !bricks.Contains(brick)) return;
        activeBricks.Remove(brick);
        if (!emptyPositions.ContainsKey(brick.GetColor()))
        {
            emptyPositions.Add(brick.GetColor(), new List<Vector3>());
        }
        if (!emptyPositions[brick.GetColor()].Contains(brick.GetStartPosition()))
        {
            emptyPositions[brick.GetColor()].Add(brick.GetStartPosition());
        }
    }
    // tra ve vi tri vien gach gan nhat so voi bot
    public Brick GetNearestBrick(Bot bot)
    {
        bool hasBrickSameColor = false;
        ColorType color = bot.GetColorType();
        Vector3 botPosition = bot.transform.position;
        Brick nearestBrick = null;
        foreach (Brick brick in activeBricks)
        {
            if (brick.GetColor() == color)
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
            if (distance <= Vector3.Distance(nearestBridge.GetTransform().position, bot.transform.position))
            {
                nearestBridge = bridge;
            }
        }
        return nearestBridge;
    }
    
    public void Despawn()
    {
        if (bricks == null || bricks.Count == 0)
        {
            return;
        }

        foreach (Brick brick in bricks)
        {
            brick.Despawn();
            brick.gameObject.SetActive(true);
            SimplePool.Despawn(brick);
        }
        bricks.Clear();
        activeBricks.Clear();
        activeColors.Clear();
        emptyPositions.Clear();
        if (bridges == null || bridges.Count == 0)
        {
            Debug.LogError("No bridge found");
            return;
        }

        foreach (Bridge bridge in bridges)
        {
            bridge.Despawn();
        }
    }
    // hien thi cac vien gach co mau chi dinh
    private void ActiveColorBricks(ColorType color)
    {
        if (activeColors.Contains(color)) return;
        activeColors.Add(color);
        foreach (Brick brick in bricks)
        {
            if (brick.GetColor() == color)
            {
                brick.gameObject.SetActive(true);
                if (!activeBricks.Contains(brick))
                {
                    activeBricks.Add(brick);
                }
            }
        }
    }
    // deactivate toan bo so gach tren stage
    private void DeactiveAllBricks()
    {
        foreach (Brick brick in bricks)
        {
            brick.gameObject.SetActive(false);
        }
    }
    // lay danh sach mau cua character
    private void SetCharacterColors(List<ColorType> colors)
    {
        this.characterColors.Clear();
        for (int i = 0; i < colors.Count; i++)
        {
            this.characterColors.Add(colors[i]);
        }
    }
}
