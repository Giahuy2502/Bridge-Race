using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasShop : UICanvas
{
    [SerializeField] List<SelectColorButton> colorButtons = new List<SelectColorButton>();
    private GameManager GameManager => GameManager.Instance;
   
    public override void Setup()
    {
        base.Setup();
        SetupColorButtons();
    }
    // khoi tao cac button color
    private void SetupColorButtons()
    {
        if (colorButtons == null || colorButtons.Count == 0)
        {
            Debug.LogError("No button assigned to UI");
            return;
        }
        for (int i = 0; i < colorButtons.Count; i++)
        {
            colorButtons[i].OnInit(this);
        }
        DeactivateAllFocus();
        ActivateCurrentColorFocus();
    }
    // tat tat ca cac focus
    public void DeactivateAllFocus()
    {
        if (colorButtons == null || colorButtons.Count == 0)
        {
            return;
        }
        for (int i = 0; i < colorButtons.Count; i++)
        {
            colorButtons[i].SetActiveFocus(false);
        }
    }
    // bat focus cho color hien tai
    private void ActivateCurrentColorFocus()
    {
        if (colorButtons == null || colorButtons.Count == 0)
        {
            return;
        }
        for (int i = 0; i < colorButtons.Count; i++)
        {
            if (colorButtons[i].GetColor() == GameManager.GetPlayerColor())
            {
                colorButtons[i].SetActiveFocus(true);
            }
        }
    }
    public void OnExitButton()
    {
        Close(0);
    }
}
