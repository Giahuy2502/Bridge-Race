using System;
using System.Collections;
using System.Collections.Generic;
using MyNamespace;
using UnityEngine;
using UnityEngine.Serialization;

public class Stair : MonoBehaviour
{
    [SerializeField] private Renderer renderer;
    [SerializeField] private ColorDataSO colorDataSO;
    [SerializeField] private float duration;
    private float timer;
    private ColorType colorType;
    public ColorType ColorType{get{return colorType;}set{colorType = value;}}

    private bool hasFilled;
    private bool isBlocked;
    private Coroutine changColorCoroutine;

    private void Start()
    {
        OnInit();
    }

    public void OnInit()
    {
        hasFilled = false;
        isBlocked = false;
        renderer.enabled = false;
        if (changColorCoroutine != null)
        {
            StopCoroutine(changColorCoroutine);
            changColorCoroutine = null;
        }
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
            else if (hasFilled && colorType != character.ColorType && character.Bricks.Count > 0)
            {
                ChangeColor(character.ColorType);
                character.RemoveBrick();
            }
            else if (hasFilled && colorType != character.ColorType && character.Bricks.Count <= 0)
            {
                
            }
        }
    }
    public void ChangeColor(ColorType colorType)
    {
        this.ColorType = colorType;
        Color newColor = colorDataSO.GetMat(colorType).color;
        if (changColorCoroutine != null)
        {
            StopCoroutine(changColorCoroutine);
        }
        changColorCoroutine = StartCoroutine(CoChangeColor(newColor, duration));
    }

    public bool CheckCanBlockPlayer(Character character)
    {
        if (hasFilled && (ColorType == character.ColorType)) return false;
        if(!hasFilled && character.Bricks.Count <=0) return true;
        if(hasFilled && (ColorType != character.ColorType) && character.Bricks.Count <= 0) return true;
        return false;
    }

    IEnumerator CoChangeColor(Color targetColor, float duration)
    {
        timer = 0;
        Material mat = renderer.material;
        while (timer <= duration)
        {
            mat.color = Color.Lerp(mat.color,targetColor, timer/this.duration);
            timer += Time.deltaTime;
            yield return null;
        }
    }
    
}
