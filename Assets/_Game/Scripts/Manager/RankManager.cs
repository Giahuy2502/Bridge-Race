using System.Collections;
using System.Collections.Generic;
using MyNamespace;
using UnityEngine;

public class RankManager : Singleton<RankManager>
{
    [SerializeField] private List<Character> finishedRankings = new List<Character>();
    [SerializeField] private List<Transform> finishPositions = new List<Transform>();
    private Dictionary<Stage,List<Character>> stageRankings = new Dictionary<Stage, List<Character>>();
    private GameManager GameManager => GameManager.Instance;
    private UIController UIController => UIController.Instance;
    public void OnInit()
    {
        finishedRankings.Clear();
        GameManager.EndGameAction += SortCharacters;
        GameManager.EndGameAction += SetFinishedPosition;
    }
    

    public void SetWinner(Character character)
    {
        if(!finishedRankings.Contains(character))
        {
            finishedRankings.Add(character);
            // Debug.Log("SetWinner: "+ character.name);
        }
    }
    private void SortCharacters()
    {
        List<Stage> keys = new List<Stage>(stageRankings.Keys);
        for (int i = keys.Count - 1; i >= 0; i--)
        {
            Stage currentKey = keys[i];
            List<Character> currentValue = stageRankings[currentKey];

            for (int j = 0; j < currentValue.Count; j++)
            {
                if (!finishedRankings.Contains(currentValue[j]))
                {
                    finishedRankings.Add(currentValue[j]);
                    // Debug.Log("SortCharacters: "+ currentValue[j].name);
                }
            }
        }
    }

    private void SetFinishedPosition()
    {
        for (int i = 0; i < finishPositions.Count; i++)
        {
            if (i>= finishedRankings.Count || finishedRankings[i] == null) return;
            finishedRankings[i].SetWinState();
            finishedRankings[i].gameObject.transform.position = finishPositions[i].position;
            finishedRankings[i].gameObject.transform.rotation = finishPositions[i].rotation;
            // Debug.Log("Set Positions: "+ finishedRankings[i].gameObject.name + " " +finishPositions[i].position);
        }
    }


    public void RegisterStagePassed(Character character, Stage stage)
    {
        if (!stageRankings.ContainsKey(stage))
        {
            stageRankings.Add(stage, new List<Character>());
        }
        if (stageRankings[stage].Contains(character))
        {
            return;
        }
        stageRankings[stage].Add(character);
        // Debug.Log("Registered Stage Passed: "+stage.name +" "+character.name);
        UIController.UpdateColorRanking();
    }

    public void SetFinishedPosition(List<Transform> positions)
    {
        finishPositions.Clear();
        foreach (Transform position in positions)
        {
            finishPositions.Add(position);
        }
    }
    private void Despawn()
    {
        
    }

    public List<ColorType> GetCurrentColorRanking()
    {
        List<ColorType> colorRanking = new List<ColorType>();
        List<Stage> keys = new List<Stage>(stageRankings.Keys);
        for (int i = keys.Count - 1; i >= 0; i--)
        {
            Stage currentKey = keys[i];
            List<Character> currentValue = stageRankings[currentKey];

            for (int j = 0; j < currentValue.Count; j++)
            {
                if (!colorRanking.Contains(currentValue[j].ColorType))
                {
                    colorRanking.Add(currentValue[j].ColorType);
                }
            }
        }
        return colorRanking;
    }

    public int GetPlayerRanking()
    {
        for (int i = 0; i < finishedRankings.Count; i++)
        {
            Character character = finishedRankings[i];
            if (character != null)
            {
                if (character is Player)
                {
                    return i;
                }
            }
        }
        return -1;
    }
    
    public bool IsPlayerLose()
    {
        return GetPlayerRanking() == (finishedRankings.Count-1);
    }
}
