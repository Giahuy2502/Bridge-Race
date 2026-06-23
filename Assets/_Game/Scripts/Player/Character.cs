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
    [SerializeField] private Transform bricksTF;
    [SerializeField] private float brickOffSetY;
    private float brickOffsetY;
    private string animName;
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

    public void AddBrick()
    {
        brickOffsetY = bricks.Count * brickOffSetY;
        Brick newBrick = SimplePool.Spawn<Brick>(PoolType.Brick,bricksTF.position + Vector3.up*brickOffsetY, bricksTF.rotation);
        newBrick.transform.parent = bricksTF;
        bricks.Add(newBrick);
        newBrick.OnInit(ColorType);
    }

    public void RemoveBrick()
    {
        if (bricks.Count <= 0) return;
        bricks[bricks.Count - 1].Despawn();
        bricks.RemoveAt(bricks.Count - 1);
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
        if (animName == anim) return;
        animator.ResetTrigger(animName);
        animName = anim;
        animator.SetTrigger(animName);
    }
    public void ChangeColor(ColorType colorType)
    {
        this.ColorType = colorType;
        renderer.material = colorDataSO.GetMat(colorType);
    }
}
