using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CanvasVictory : UICanvas
{
    [SerializeField] private List<Image> starts;
    private int playerRank = 0;
    private GameManager GameManager => GameManager.Instance;
    private RankManager RankManager => RankManager.Instance;

    public override void Setup()
    {
        base.Setup();
        playerRank = GetCountStar(RankManager.GetPlayerRanking());
        SetStar(playerRank);
    }
    public void MainMenuButton()
    {
        GameManager.OnMainMenu();
        DisableAllStars();
        Close(0);
        UIManager.Instance.Open<CanvasMainMenu>();
    }

    private void SetStar(int count)
    {
        for (int i = 0; i < count; i++)
        {
            starts[i].gameObject.SetActive(true);
        }
    }

    private int GetCountStar(int playerRank)
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

    private void DisableAllStars()
    {
        for (int i = 0; i < starts.Count; i++)
        {
            starts[i].gameObject.SetActive(false);
        }
    }
}
