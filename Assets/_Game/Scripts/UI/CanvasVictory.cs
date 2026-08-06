using System.Collections;
using System.Collections.Generic;
using MyNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CanvasVictory : UICanvas
{
    [SerializeField] private List<Image> starts;
    private int numStar = 0;
    private GameManager GameManager => GameManager.Instance;
    private LevelManager LevelManager => LevelManager.Instance;
    private UIController UIController => UIController.Instance;
    
    // ham quay lai main menu
    public void MainMenuButton()
    {
        LevelManager.SetNextLevel();
        GameManager.OnMainMenu();
        Close(0);
        UIController.ShowMenu();
    }
    // chuyen sang level tiep theo
    public void NextLevelButton()
    {
        GameManager.NextLevel();
        Close(0);
    }
    // set star
    public void SetStar(int count)
    {
        DisableAllStars();
        for (int i = 0; i < count; i++)
        {
            starts[i].gameObject.SetActive(true);
        }
    }
    
    // tat toan bo star
    private void DisableAllStars()
    {
        for (int i = 0; i < starts.Count; i++)
        {
            starts[i].gameObject.SetActive(false);
        }
    }
}
