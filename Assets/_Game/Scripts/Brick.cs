using System.Collections;
using System.Collections.Generic;
using MyNamespace;
using UnityEngine;

public class Brick : GameUnit
{
    [SerializeField] Renderer renderer;
    [SerializeField] ColorDataSO colorDataSO;
    private ColorType colorType;
    public ColorType ColorType { get; private set;}
    public void OnInit(ColorType color)
    {
        ChangeColor(color);
    }

    public void Despawn()
    {
        
    }

    public void ChangeColor(ColorType colorType)
    {
        this.colorType = colorType;
        renderer.material = colorDataSO.GetMat(colorType);
    }
}
