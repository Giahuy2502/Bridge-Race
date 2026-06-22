using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyNamespace;
[CreateAssetMenu(fileName = "ColorDataSO", menuName = "ColorDataSO", order = 1)]
public class ColorDataSO : ScriptableObject
{
    [SerializeField] private Material[] materials;

    public Material GetMat(ColorType color)
    {
        return materials[(int)color];
    }
}
