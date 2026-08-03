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
    [SerializeField] private ColorType colorType;
    [SerializeField] private bool hasFilled;
    [SerializeField] private bool isBlocked;
    [SerializeField] private Stage stage;
    private float timer;
    private Coroutine changColorCoroutine;
    public void OnInit(Stage stage)
    {
        hasFilled = false;
        isBlocked = false;
        colorType = ColorType.None;
        renderer.material = colorDataSO.GetMat(ColorType.white);
        renderer.enabled = false;
        if (changColorCoroutine != null)
        {
            StopCoroutine(changColorCoroutine);
            changColorCoroutine = null;
        }
        this.stage = stage;
    }
    public void OnTriggerEnter(Collider other)
    {
        Character character = MyCache.GetCharacter<Character>(other);
        if (character != null && stage == character.GetStage())
        {
            if (!hasFilled && character.GetListBricksCount() > 0)
            {
                renderer.enabled = true;
                ChangeColor(character.GetColorType());
                character.RemoveBrick();
                hasFilled = true;
            }
            else if(!hasFilled && character.GetListBricksCount() < 0)
            {
                isBlocked = true;
            }
            else if (hasFilled && colorType != character.GetColorType() && character.GetListBricksCount() > 0)
            {
                ChangeColor(character.GetColorType());
                character.RemoveBrick();
            }
        }
    }
    // ham chuyen mau colorType
    private void ChangeColor(ColorType colorType)
    {
        this.colorType = colorType;
        Color newColor = colorDataSO.GetMat(colorType).color;
        if (changColorCoroutine != null)
        {
            StopCoroutine(changColorCoroutine);
        }
        changColorCoroutine = StartCoroutine(CoChangeColor(newColor, duration));
    }
    // ham kiem tra xem co chan duoc player ko
    public bool CheckCanBlockPlayer(Character character)
    {
        if (hasFilled && (colorType == character.GetColorType())) return false;
        if(!hasFilled && character.GetListBricksCount() <=0) return true;
        if(hasFilled && (colorType != character.GetColorType()) && character.GetListBricksCount() <= 0) return true;
        return false;
    }

    IEnumerator CoChangeColor(Color targetColor, float duration)
    {
        timer = 0;
        Material mat = renderer.material;
        while (timer <= duration)
        {
            mat.color = Color.Lerp(mat.color,targetColor, timer/duration);
            timer += Time.deltaTime;
            yield return null;
        }
    }

    public void Despawn()
    {
        hasFilled = false;
        isBlocked = false;
        renderer.enabled = false;
        colorType = ColorType.None;
        if (changColorCoroutine != null)
        {
            StopCoroutine(changColorCoroutine);
            changColorCoroutine = null;
        }
    }

    public ColorType GetColorType()
    {
        return colorType;
    }

    public bool HasFilled()
    {
        return hasFilled;
    }
}
