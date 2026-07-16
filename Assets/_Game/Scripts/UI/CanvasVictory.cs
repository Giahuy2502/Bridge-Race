using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class CanvasVictory : UICanvas
{
    [SerializeField] private TextMeshProUGUI scoreText;
    private GameManager GameManager => GameManager.Instance;
    public void SetBestScore(int coin)
    {
        scoreText.text = coin.ToString();
    }
    public void MainMenuButton()
    {
        GameManager.OnMainMenu();
        Close(0);
        UIManager.Instance.Open<CanvasMainMenu>();
    }
    
}
