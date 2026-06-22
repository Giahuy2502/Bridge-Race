using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanvasFail : UICanvas
{
    [SerializeField] private TextMeshProUGUI scoreText;
    
    public void SetBestScore(int coin)
    {
        scoreText.text = coin.ToString();
    }
    public void MainMenuButton()
    {
        Close(0);
        UIManager.Instance.Open<CanvasMainMenu>();
    }
}
