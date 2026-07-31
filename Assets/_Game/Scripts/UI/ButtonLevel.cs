using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonLevel : MonoBehaviour
{
    [SerializeField] private List<Image> stars = new List<Image>();
    [SerializeField] private List<GameObject> stage = new List<GameObject>();
    [SerializeField] private GameObject focus;
    
    private CanvasMainMenu canvasMainMenu;
    private LevelManager LevelManager => LevelManager.Instance;
    public void OnInit(CanvasMainMenu canvasMainMenu)
    {
        this.canvasMainMenu = canvasMainMenu;
        DeactivateStars();
    }

    private void SetStage(int stageIndex)
    {
        if (stage == null || stage.Count == 0)
        {
            Debug.LogError("No Stages assigned to ButtonLevel");
            return;
        }

        for (int i = 0; i < stage.Count; i++)
        {
            if (i == stageIndex)
            {
                stage[i].gameObject.SetActive(true);
            }
            else
            {
                stage[i].gameObject.SetActive(false);
            }
        }
    }

    public void ClickLevelButton(int levelIndex)
    {
        Debug.Log("OnClick " + this.name);
        canvasMainMenu.DeactivateAllFocus();
        SetActiveFocus(true);
        Debug.Log("OnClick " + this.name);
        LevelManager.CurrentLevel = levelIndex;
    }

    public void SetActiveFocus(bool active)
    {
        focus.SetActive(active);
    }

    private void DeactivateStars()
    {
        if (stars == null || stars.Count == 0)
        {
            Debug.LogError("No stars !!!");
            return;
        }
        for (int i = 0; i < stars.Count; i++)
        {
            stars[i].gameObject.SetActive(false);
        }
    }
}
