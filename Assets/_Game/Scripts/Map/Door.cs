using System;
using System.Collections;
using System.Collections.Generic;
using MyNamespace;
using UnityEngine;
using UnityEngine.Serialization;

public class Door : MonoBehaviour
{
    [SerializeField] private ColorDataSO colorDataSO;
    [SerializeField] private Animator animator;
    [SerializeField] private Renderer[] renderers;
    private ColorType colorType;
    private Stage stage;
    private bool isOpened = false;
    public Stage Stage{get{return stage;} set{stage = value;}}
    

    public void OnInit(Stage stage)
    {
        this.stage = stage;
        isOpened = false;
    }

    public void OnTriggerEnter(Collider other)
    {
        Character character = MyCache.GetCharacter<Character>(other);
        if (!isOpened && character != null && stage == character.Stage)
        {
            isOpened = true;
            ChangeColor(character.ColorType);
            PlayOpenAnim();
        }
    }

    private void ChangeColor(ColorType colorType)
    {
        this.colorType = colorType;
        foreach (Renderer renderer in renderers)
        {
            renderer.material = colorDataSO.GetMat(colorType);
        }
    }
    private void PlayOpenAnim()
    {
        animator.ResetTrigger(Variables.OPEN_ANIM);
        animator.SetTrigger(Variables.OPEN_ANIM);
    }

    private void ResetAnimator()
    {
        animator.Rebind();
        animator.Update(0f);
    }

    public void Despawn()
    {
        isOpened = true;
        ChangeColor(ColorType.white);
        ResetAnimator();
    }
}
