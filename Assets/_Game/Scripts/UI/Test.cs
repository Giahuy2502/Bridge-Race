using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    int score = 0;
    // Start is called before the first frame update
    void Start()
    {
        UIManager.Instance.Open<CanvasMainMenu>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q) && UIManager.Instance.IsOpened<CanvasGamePlay>())
        {
            UIManager.Instance.GetUI<CanvasGamePlay>().UpdateCoin(++score);
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            UIManager.Instance.ClodeAll();
            UIManager.Instance.Open<CanvasVictory>();
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            UIManager.Instance.ClodeAll();
            UIManager.Instance.Open<CanvasFail>().SetBestScore(score);
        }
    }
}
