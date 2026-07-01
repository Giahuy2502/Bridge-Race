using System;
using System.Collections;
using System.Collections.Generic;
using MyNamespace;
using UnityEngine;
using UnityEngine.Serialization;

public class Door : MonoBehaviour
{
    [SerializeField] private ColorDataSO colorDataSO;
    [SerializeField] private Renderer[] renderers;
    private ColorType colorType;
    private Stage stage;
    public Stage Stage{get{return stage;} set{stage = value;}}
    

    public void OnInit(Stage stage)
    {
        this.stage = stage;
    }

    public void OnTriggerEnter(Collider other)
    {
        Character character = MyCache.GetCharacter<Character>(other);
        if (character != null && stage == character.Stage)
        {
            ChangeColor(character.ColorType);
        }
    }

    public void ChangeColor(ColorType colorType)
    {
        this.colorType = colorType;
        foreach (Renderer renderer in renderers)
        {
            renderer.material = colorDataSO.GetMat(colorType);
        }
    }

    public void Despawn()
    {
        
    }
}
