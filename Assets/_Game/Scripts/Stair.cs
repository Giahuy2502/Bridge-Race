using System;
using System.Collections;
using System.Collections.Generic;
using MyNamespace;
using UnityEngine;

public class Stair : MonoBehaviour
{
    [SerializeField] private Renderer renderer;
    [SerializeField] private ColorDataSO colorDataSO;
    private ColorType colorType;
    public ColorType ColorType{get{return colorType;}set{colorType = value;}}

    private bool hasFilled;
    private bool isBlocked;

    private void Start()
    {
        OnInit();
    }

    public void OnInit()
    {
        hasFilled = false;
        isBlocked = false;
        renderer.enabled = false;
    }
    public void OnTriggerEnter(Collider other)
    {
        Character character = MyCache.GetCharacter<Character>(other);
        if (character != null)
        {
            if (!hasFilled && character.Bricks.Count > 0)
            {
                renderer.enabled = true;
                ChangeColor(character.ColorType);
                character.RemoveBrick();
                hasFilled = true;
            }
            else if(!hasFilled && character.Bricks.Count < 0)
            {
                isBlocked = true;
            }
            else if (hasFilled && colorType != character.ColorType && character.Bricks.Count >= 2)
            {
                character.RemoveBrick();
                ChangeColor(character.ColorType);
                character.RemoveBrick();
            }
            else if (hasFilled && colorType != character.ColorType && character.Bricks.Count < 2)
            {
                
            }
        }
    }
    public void ChangeColor(ColorType colorType)
    {
        this.ColorType = colorType;
        renderer.material = colorDataSO.GetMat(colorType);
    }

    public bool CheckCanBlockPlayer(Character character)
    {
        if (hasFilled && (ColorType == character.ColorType)) return false;
        if(!hasFilled && character.Bricks.Count <=0) return true;
        if(hasFilled && (ColorType != character.ColorType) && character.Bricks.Count < 2) return true;
        return false;
    }
    
}
