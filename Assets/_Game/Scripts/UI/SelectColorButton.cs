using System.Collections;
using System.Collections.Generic;
using MyNamespace;
using UnityEngine;

public class SelectColorButton : MonoBehaviour
{
    [SerializeField] private GameObject focus;
    [SerializeField] private ColorType color;
    private GameManager GameManager => GameManager.Instance;
    private CanvasShop canvasShop;
    public void OnInit(CanvasShop canvasShop)
    {
        this.canvasShop = canvasShop;
    }
    
    public void SelectColor()
    {
        canvasShop.DeactivateAllFocus();
        SetActiveFocus(true);
        GameManager.SetPlayerColor(color);
    }

    public void SetActiveFocus(bool active)
    {
        focus.SetActive(active);
    }
    public ColorType GetColor()
    {
        return color;
    }
}
