using System;
using System.Collections;
using System.Collections.Generic;
using MyNamespace;
using UnityEngine;
using UnityEngine.AI;

public class Bot : Character
{
    [SerializeField] private int maxBrick;
    [SerializeField] NavMeshAgent agent;
    [Header("State Field")]
    [SerializeField] private float idleTime = 1f;
    private float time = 0;
    private IState currentState;
    
    private Bridge nearestBridge;
    private int indexStair = 0;
    private bool isApproachingBridge = true;
    private bool isCrossing = false;
    private Vector3 highestStairPos;
    private int stairWalkeableCount = 0;
    
    public override void OnInit(ColorType colorType)
    {
        base.OnInit(colorType);
        this.name = "Bot-"+colorType.ToString();
        SetAbleAgent(true);
        ChangeState(new IdleState());
    }
    public override void Despawn()
    {
        base.Despawn();
        SetAbleAgent(false);
        this.gameObject.SetActive(false);
    }
    private void Update()
    {
        if (!IsPlaying())
        {
            PauseAgent(true);
            return;
        }
        if (IsPlaying())
        {
            PauseAgent(false);
        }
        if (currentState != null)
        {
            currentState.OnExcute(this);
        }
    }
    public void ChangeState(IState newState)
    {
        if (currentState != null)
        {
            currentState.OnExit(this);
        }
        currentState = newState;
        if (currentState != null)
        {
            currentState.OnEnter(this);
        }
        // Debug.Log("ChangeState: "+ currentState);
    }
    public void SetDestination(Vector3 destination)
    {
        agent.SetDestination(destination);
        ChangeAnim(Variables.RUN_ANIM);
    }
    public void StopMove()
    {
        agent.velocity = Vector3.zero;
        ChangeAnim(Variables.IDLE_ANIM);
    }
    public Brick GetNearestBrick()
    {
        return stage.GetNearestBrick(this);
    }
    public Vector3 GetNearestBrickPos(Brick brick)
    {
        if(brick == null) return Vector3.zero;
        return brick.transform.position;
    }
    public Bridge GetNearestBridge()
    {
        this.nearestBridge = stage.GetNearestBridge(this);
        return nearestBridge;
    }

    public Bridge NearestBridge()
    {
        return nearestBridge;
    }
    
    // kiem tra xem bot den vi tri destination chua
    public bool IsReachedDestination()
    {
        if (agent.pathPending)
        {
            return false;
        }
        if (agent.remainingDistance > agent.stoppingDistance)
        {
            return false;
        }
        if (agent.hasPath && agent.velocity.sqrMagnitude != 0f)
        {
            return false;
        }
        return true;
    }
    private int GetStairWalkable(int brickCount)
    {
        return stage.GetNearestBridge(this).GetStairWalkable(this.colorType, brickCount);
    }
    public override void SetWinState()
    {
        if (currentState != null)
        {
            currentState.OnExit(this);
            currentState = null;
        }
        SetAbleAgent(false);
        base.SetWinState();
    }
    public void SetBuildState(bool isBuilding)
    {
        if (isBuilding)
        {
            agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        }
        else
        {
            agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
        }
    }

    private void SetAbleAgent(bool isAble)
    {
        if (isAble)
        {
            agent.enabled = true;
        }
        else
        {
            if (agent != null && agent.isOnNavMesh)
            {
                agent.ResetPath();
                agent.velocity = Vector3.zero;
                agent.isStopped = true;
                agent.enabled = false;
            }
            else if (agent != null)
            {
                agent.enabled = false; 
            }
        }
    }

    private void PauseAgent(bool isPaused)
    {
        if (isPaused)
        {
            if (agent != null && agent.isActiveAndEnabled && agent.isOnNavMesh && !agent.isStopped)
            {
                agent.isStopped = true;
                agent.velocity = Vector3.zero;
            }
        }
        else
        {
            if (agent != null && agent.isStopped)
            {
                agent.isStopped = false;
            }
        }
    }
    public int GetMaxBrick()
    {     
        return maxBrick;
    }
    // ham lay vi tri bac thang cao nhat co the di
    public void SetHighestStairPos()
    {
        stairWalkeableCount = GetStairWalkable(GetListBricksCount());
        if (stairWalkeableCount <= 0)
        {
            StopMove();
        }
        indexStair = stairWalkeableCount - 1;
        if (!ValidIndex(indexStair))
        {
            StopMove();
        }
        highestStairPos = nearestBridge.GetStairs()[indexStair].transform.position + Vector3.up * 0.25f;
    }
    // ham xem chi so stair co hop li ko
    private bool ValidIndex(int index)
    {
        if (nearestBridge == null)
        {
            return false;
        }
        return index >= 0 && index < nearestBridge.GetStairs().Count;
    }
    public float GetTimer()
    {
        return time;
    }

    public void SetTimer(float time)
    {
        this.time = time;
    }
    public void AddToTimer(float amount)
    {
        time += amount;
    }
    public float GetIdleTime()
    {
        return idleTime;
    }
    
    public void SetHighestStairPos(Vector3 pos)
    {
        highestStairPos = pos;
    }
    public int GetStairWalkeableCount()
    {
        return stairWalkeableCount;
    }

    public void SetStairWalkeableCount(int count)
    {
        stairWalkeableCount = count;
    }
    public bool IsCrossing()
    {
        return isCrossing;
    }

    public void SetIsCrossing(bool isCrossing)
    {
        this.isCrossing = isCrossing;
    }
    public bool IsApproachingBridge()
    {
        return isApproachingBridge;
    }

    public void SetIsApproachingBridge(bool isApproachingBridge)
    {
        this.isApproachingBridge = isApproachingBridge;
    }

    [ContextMenu("Show Destination")]
    public void ShowDestination()
    {
        Debug.Log("Show Destination: Transform: " + transform.position + ",Destibation: " + agent.destination);
    }

    public Vector3 GetHighestStairPos()
    {
        return highestStairPos;
    }
}
