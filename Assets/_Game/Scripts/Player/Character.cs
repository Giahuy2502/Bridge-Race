using System;
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
    private Stage stage;
    private float brickOffsetY;
    private string animName;
    private Transform startPos;
    public ColorType ColorType { get; private set;}
    public List<Brick> Bricks { get => bricks; private set => bricks = value; }
    public Transform BricksTF { get => bricksTF; private set => bricksTF = value; }
    public Stage Stage { get => stage; set => stage = value; }
    public GameManager GameManager => GameManager.Instance;
    private SoundManager SoundManager => SoundManager.Instance;

    public virtual void OnInit(ColorType colorType)
    {
        this.gameObject.SetActive(true);
        bricksTF.gameObject.SetActive(true);
        ChangeColor(colorType);
    }

    public void SetStartPoints(Transform startPoints)
    {
        this.transform.position = startPoints.position;
        this.transform.rotation = startPoints.rotation;
        startPos = startPoints;
    }
    public virtual void Despawn()
    {
        ClearAllBricks();
        stage = null;
        animator.Rebind();
    }
    public void AddBrick()
    {
        Vector3 newpos = GetNewestBrickPos();
        Brick newBrick = SimplePool.Spawn<Brick>(PoolType.Brick,bricksTF.position + newpos, bricksTF.rotation);
        BrickEffect newBrickEffect = SimplePool.Spawn<BrickEffect>(PoolType.BrickEffect, bricksTF.position + newpos, bricksTF.rotation);
        newBrick.transform.parent = bricksTF;
        newBrick.transform.localPosition =  newpos;
        newBrickEffect.transform.parent = bricksTF;
        newBrickEffect.transform.localPosition =  newpos;
        bricks.Add(newBrick);
        // Debug.Log("Brick added brick :" + bricks.Count);
        newBrick.OnInit(ColorType, this.Stage);
        newBrickEffect.PlayBrickEffect(this.ColorType);
        PlaySFX(FxID.SFX_CollectBrick);
    }
    public void RemoveBrick()
    {
        if (bricks.Count <= 0) return;
        bricks[bricks.Count - 1].Despawn();
        bricks.RemoveAt(bricks.Count - 1);
        // Debug.Log("Brick remove brick :" + bricks.Count);
        Stage.RespawnBrick(ColorType);
        PlaySFX(FxID.SFX_BuildBridge);
    }
    private void ClearAllBricks()
    {
        while (bricks.Count > 0)
        {
            bricks[bricks.Count - 1].Despawn();
            bricks.RemoveAt(bricks.Count - 1);
            Stage.RespawnBrick(ColorType);
        }
        bricksTF.gameObject.SetActive(false);
        // Debug.Log("Bricks destroyed: "+ bricks.Count+" "+this.name);
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

    public virtual void SetWinState()
    {
        ClearAllBricks();
        ChangeAnim(Variables.CHEER_ANIM);
    }

    public bool IsPlaying()
    {
        return GameManager.GameState == GameState.Playing;
    }

    private void PlaySFX(FxID sfxID)
    {
        SoundManager.PlayFx(sfxID);
    }

    public Transform GetStartPos()
    {
        if (startPos == null) return tf;
        return startPos;
    }
}
