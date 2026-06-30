using System;
using System.Collections;
using System.Collections.Generic;
using MyNamespace;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private ColorDataSO colorDataSO;
    [SerializeField] private Renderer renderer;
    private ColorType colorType;

    private void Start()
    {
        OnInit();
    }

    public void OnInit()
    {
        this.renderer.enabled = false;
    }

    public void OnTriggerEnter(Collider other)
    {
        Character character = MyCache.GetCharacter<Character>(other);
        if (character != null)
        {
            ChangeColor(character.ColorType);
        }
    }

    public void ChangeColor(ColorType colorType)
    {
        this.renderer.enabled = true;
        this.colorType = colorType;
        renderer.material = colorDataSO.GetMat(colorType);
    }

    public void Despawn()
    {
        
    }
}
