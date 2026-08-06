using System;
using System.Collections;
using System.Collections.Generic;
using MyNamespace;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private List<Brick> bricks = new List<Brick>();
    [SerializeField] private Animator animator;
    [SerializeField] private Renderer renderer;
    [SerializeField] private ColorDataSO colorDataSO;
    [SerializeField] protected Transform tf;
    [SerializeField] private Transform bricksTF;
    [SerializeField] private float brickOffSetY;
    [SerializeField] protected Stage stage;
    protected ColorType colorType;
    private float brickOffsetY;
    private string animName;
    private Transform startPos;
    private GameManager GameManager => GameManager.Instance;
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
    // ham them brick moi vao sau character
    public virtual void AddBrick()
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
        newBrick.OnInit(colorType, this.stage);
        newBrickEffect.PlayBrickEffect(this.colorType);
    }
    // ham xoa brick sau lung character
    public virtual void RemoveBrick()
    {
        if (bricks.Count <= 0) return;
        bricks[bricks.Count - 1].Despawn();
        bricks.RemoveAt(bricks.Count - 1);
        // Debug.Log("Brick remove brick :" + bricks.Count);
        stage.RespawnBrick(colorType);
    }
    // ham xoa tat ca brick sau lung character
    private void ClearAllBricks()
    {
        while (bricks.Count > 0)
        {
            bricks[bricks.Count - 1].Despawn();
            bricks.RemoveAt(bricks.Count - 1);
            stage.RespawnBrick(colorType);
        }
        bricksTF.gameObject.SetActive(false);
        // Debug.Log("Bricks destroyed: "+ bricks.Count+" "+this.name);
    }
    protected void ChangeAnim(string anim)
    {
        if (animName == anim) return;
        animator.ResetTrigger(animName);
        animName = anim;
        animator.SetTrigger(animName);
    }
    private void ChangeColor(ColorType colorType)
    {
        this.colorType = colorType;
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
    // kiem tra xem co dang trong gameplay hay ko
    protected bool IsPlaying()
    {
        return GameManager.GetGameState() == GameState.Playing;
    }
    protected void PlaySFX(FxID sfxID)
    {
        SoundManager.PlayFx(sfxID);
    }
    // ham lay vi tri ban dau trong map
    protected Transform GetStartPos()
    {
        if (startPos == null) return tf;
        return startPos;
    }
    public Stage GetStage()
    {
        return stage;
    }
    public void SetStage(Stage stage)
    {
        this.stage = stage;
    }
    public ColorType GetColorType()
    {
        return colorType;
    }
    public int GetListBricksCount()
    {
        return bricks.Count;
    }
    public Transform GetBricksTF()
    {
        return bricksTF;
    }
}
