using System.Collections;
using System.Collections.Generic;
using MyNamespace;
using UnityEngine;

public class CanvasShop : UICanvas
{
    [SerializeField] List<SelectColorButton> colorButtons = new List<SelectColorButton>();
    private ColorType playerColor;
    public void OnInit(ColorType playerColor)
    {
        this.playerColor = playerColor;
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
            if (colorButtons[i].GetColor() == playerColor)
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
