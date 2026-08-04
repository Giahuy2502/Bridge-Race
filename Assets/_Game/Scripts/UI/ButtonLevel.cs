using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonLevel : MonoBehaviour
{
    [SerializeField] private List<Image> stars = new List<Image>();
    [SerializeField] private List<GameObject> stage = new List<GameObject>();
    [SerializeField] private GameObject focus;
    private bool isUnlocked = false;
    
    private CanvasMainMenu canvasMainMenu;
    private LevelManager LevelManager => LevelManager.Instance;
    public void OnInit(CanvasMainMenu canvasMainMenu, bool isUnlocked)
    {
        this.canvasMainMenu = canvasMainMenu;
        this.isUnlocked = isUnlocked;
        DeactivateStars();
    }
    // set trang thai cua stage
    public void SetDefaulStage(bool isUnlocked)
    {
        if (stage == null || stage.Count == 0)
        {
            Debug.LogError("No Stages assigned to ButtonLevel");
            return;
        }
        this.isUnlocked = isUnlocked;
        if (!this.isUnlocked)
        {
            stage[0].SetActive(true);
            stage[1].SetActive(false);
            stage[2].SetActive(false);
        }
        else
        {
            stage[0].SetActive(false);
            stage[1].SetActive(true);
            stage[2].SetActive(false);
        }
    }
    // set trang thai duoc bam
    public void SetSelectStage()
    {
        stage[0].SetActive(false);
        stage[1].SetActive(false);
        stage[2].SetActive(true);
    }

    public void ClickLevelButton(int levelIndex)
    {
        if (!isUnlocked)
        {
            return;
        }
        canvasMainMenu.DeactivateAllFocus();
        canvasMainMenu.SetDefaultLevelStage();
        SetActiveFocus(true);
        SetSelectStage();
        LevelManager.SetCurrentLevel(levelIndex);
    }
    // ham set focus
    public void SetActiveFocus(bool active)
    {
        focus.SetActive(active);
    }
    // tat tat ca sao
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
    // bat sao
    public void ActivateStars(int starsCount)
    {
        for (int i = 0; i < starsCount; i++)
        {
            stars[i].gameObject.SetActive(true);
        }
    }
}
