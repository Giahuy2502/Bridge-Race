using System.Collections;
using System.Collections.Generic;
using MyNamespace;
using UnityEngine;

public class BrickEffect : GameUnit
{
    [SerializeField] private ColorDataSO colorDataSO;
    [SerializeField] private ParticleSystem brickEffect;
    [SerializeField] private float duration;
   
    private ColorType color;
    
    public void PlayBrickEffect(ColorType color)
    {
        ChangeColor(color);
        brickEffect.Play();
        Invoke(nameof(Despawn), duration);
    }

    public void Despawn()
    {
        SimplePool.Despawn(this);
    }
    private void ChangeColor(ColorType colorType)
    {
        if(color == colorType) return;
        this.color = colorType;
        ParticleSystem.MainModule main = brickEffect.main;
        main.startColor = colorDataSO.GetMat(colorType).color;
    }
}
