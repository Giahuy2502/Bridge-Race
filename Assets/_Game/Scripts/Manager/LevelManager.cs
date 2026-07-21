using System.Collections;
using System.Collections.Generic;
using MyNamespace;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private List<Character> characters = new List<Character>();
    [SerializeField] private List<Transform> startPoints;
    [SerializeField] private Transform brickParent;
    [SerializeField] private int currentLevel = 0;
    private RankManager RankManager => RankManager.Instance;
    private GameManager GameManager => GameManager.Instance;
    
    private DataManager DataManager => DataManager.Instance;
    public List<Character> Characters { get => characters; set => characters = value; }
    public Transform BrickParent { get => brickParent; set => brickParent = value; }
    public void OnInit()
    {
        RankManager.OnInit();
        // GameManager.WinAction += OnPause;
    }

    public void LoadLevel()
    {
        // data manager load map
        DataManager.LoadLevel(currentLevel -1);
        // khoi tao character
        StartCoroutine(ISpawnCharactersRoutine(startPoints));
    }

    public void OnPlay()
    {
        GameManager.ChangeState(GameState.Playing);
    }

    public void OnPause()
    {
        Time.timeScale = 0f;
    }

    public void OnContinue()
    {
        Time.timeScale = 1f;
    }

    public void OnDespawn()
    {
        DataManager.DespawnLevel(currentLevel -1);
    }

    public void OnWin()
    {
        
    }

    public void OnLose()
    {
       
    }
    // ham goi khi choi lai level hien tai

    public void OnRestart()
    {
        OnDespawn();
        LoadLevel();
        OnInit();
        OnPlay();
    }
    // ham goi khi chuyen sang level tiep theo
    public void OnNext()
    {
        OnDespawn();
        LoadLevel();
        OnInit();
        OnPlay();
    }

    private void SpawnCharacters(List<Transform> spawnPoints)
    {
        if (characters == null || characters.Count == 0)
        {
            Debug.LogError("No characters assigned");
            return;
        }

        if (startPoints == null || startPoints.Count == 0 || startPoints.Count != characters.Count)
        {
            Debug.LogError("No startPoints assigned or startPoints don't match characters");
            return;
        }

        for (int i = 0; i < characters.Count; i++)
        {
            Character character = characters[i];
            Transform startPoint = startPoints[i];
            character.OnInit(startPoint);
        }
    }

    IEnumerator ISpawnCharactersRoutine(List<Transform> spawnPoints)
    {
        yield return new WaitForSeconds(0.25f);
        SpawnCharacters(spawnPoints);
    }
}
