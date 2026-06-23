using System.Collections;
using System.Collections.Generic;
using MyNamespace;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] List<Brick> bricks = new List<Brick>();
    [SerializeField] private Animator animator;
    [SerializeField] Renderer renderer;
    [SerializeField] ColorDataSO colorDataSO;
    [SerializeField] protected Transform tf;
    private ColorType colorType;
    public ColorType ColorType { get; private set;}

    public virtual void OnInit()
    {
        
    }
    protected void Start()
    {
        ChangeColor(ColorType.Green);
    }

    public virtual void OnDespawn()
    {
        
    }

    public virtual void AddBrick(Brick brick)
    {
        
    }

    public virtual void RemoveBrick(Brick brick)
    {
        
    }

    public virtual void ClearBricks()
    {
        
    }

    public virtual void Move(Vector3 targetPos)
    {
        RotateToTarget(targetPos);
    }

    public virtual void RotateToTarget(Vector3 targetPos)
    {
        
    }

    public void ChangeAnim(string anim)
    {
        animator.SetTrigger(anim);
    }
    public void ChangeColor(ColorType colorType)
    {
        this.colorType = colorType;
        renderer.material = colorDataSO.GetMat(colorType);
    }
}
