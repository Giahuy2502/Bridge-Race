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
    
    public void SetupColorButtons()
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

    private void ActivateCurrentColorFocus()
    {
        if (colorButtons == null || colorButtons.Count == 0)
        {
            return;
        }
        for (int i = 0; i < colorButtons.Count; i++)
        {
            if (colorButtons[i].Color == GameManager.PlayerColor)
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
