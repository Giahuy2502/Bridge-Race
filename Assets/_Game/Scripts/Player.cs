using System;
using System.Collections;
using System.Collections.Generic;
using MyNamespace;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Renderer renderer;
    [SerializeField] ColorDataSO colorDataSO;
    private ColorType colorType;
    public ColorType ColorType { get; private set;}

    private void Start()
    {
        ChangeColor(ColorType.Green);
    }

    public void ChangeColor(ColorType colorType)
    {
        this.colorType = colorType;
        renderer.material = colorDataSO.GetMat(colorType);
    }
}
