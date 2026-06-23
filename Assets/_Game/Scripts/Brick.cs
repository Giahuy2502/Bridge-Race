using System;
using System.Collections;
using System.Collections.Generic;
using MyNamespace;
using UnityEngine;

public class Brick : GameUnit
{
    [SerializeField] Renderer renderer;
    [SerializeField] ColorDataSO colorDataSO;
    private Stage stage;
    private bool isTaked = false;
    public ColorType ColorType { get; private set;}
    public Stage Stage { get => stage;set => stage = value; }
    public void OnInit(ColorType color)
    {
        ChangeColor(color);
    }

    public void Despawn()
    {
        ChangeColor(ColorType.None);
        if(stage != null) stage.Despawn(this);
    }

    public void ChangeColor(ColorType colorType)
    {
        this.ColorType = colorType;
        renderer.material = colorDataSO.GetMat(colorType);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!isTaked && other.CompareTag(Variables.PLAYER_TAG))
        {
            Character newChar = other.gameObject.GetComponent<Character>();
            Debug.Log(newChar.ColorType);
            Player player = MyCache.GetCharacter<Player>(other);
            Debug.Log("Player entered:" + player.ColorType + ", " + ColorType);
            if (ColorType != player.ColorType) return;
            player.AddBrick();
            Despawn();
            isTaked = true;
        }
    }
}
