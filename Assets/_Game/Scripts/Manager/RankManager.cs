using System.Collections;
using System.Collections.Generic;
using MyNamespace;
using Unity.VisualScripting;
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
        stageRankings.Clear();
        GameManager.ResetEndGameAction();
        GameManager.GetEndGameAction().AddListener(SortCharacters);
        GameManager.GetEndGameAction().AddListener(SetFinishedPosition);
    }
    // ham duoc goi de set nguoi dung 1
    public void SetWinner(Character character)
    {
        if(!finishedRankings.Contains(character))
        {
            finishedRankings.Add(character);
        }
    }
    // ham duoc goi de sort cac character con lai
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
                }
            }
        }
    }
    // ham set vi tri cac character ve cac winner stage
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
    // ham duoc goi moi khi nhan vat di vao stage moi
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
        UIController.UpdateColorRanking();
    }
    // ham set finished position
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
    // ham lay xep hang mau (dung cho UI ranking)
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
                if (!colorRanking.Contains(currentValue[j].GetColorType()))
                {
                    colorRanking.Add(currentValue[j].GetColorType());
                }
            }
        }
        return colorRanking;
    }
    // lay vi tri xep hang cua player
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
    // ham kiem tra xem player co thua hay ko
    public bool IsPlayerLose()
    {
        return GetPlayerRanking() == (finishedRankings.Count-1);
    }
    // ham lay so sao tu xep hang nguoi choi
    public int GetCountStar(int playerRank)
    {
        switch (playerRank)
        {
            case 0:
                return 3;
            case 1:
                return 2;
            case 2:
                return 1;
            default:
                return 0;
        }
    }
}
