using System.Collections;
using System.Collections.Generic;
using MyNamespace;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private ColorType targetColor;
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
    public List<Brick> Bricks { get => bricks; private set => bricks = value; }
    public Transform BricksTF { get => bricksTF; private set => bricksTF = value; }

    public virtual void OnInit()
    {
        ChangeColor(targetColor);
    }
    protected void Start()
    {
        OnInit();
    }
    public virtual void OnDespawn()
    {
        
    }
    public void AddBrick()
    {
        Vector3 newpos = GetNewestBrickPos();
        Brick newBrick = SimplePool.Spawn<Brick>(PoolType.Brick,bricksTF.position + newpos, bricksTF.rotation);
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

    // tra ve vi tri cua brick moi so voi brickTF
    public Vector3 GetNewestBrickPos()
    {
        brickOffsetY = bricks.Count * brickOffSetY;
        return Vector3.up * brickOffsetY;
    }
}
