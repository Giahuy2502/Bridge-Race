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
    private IState currentState;

    public override void OnInit(ColorType colorType)
    {
        base.OnInit(colorType);
        this.name = "Bot-"+colorType.ToString();
        agent.enabled = true;
        ChangeState(new IdleState());
    }

    public override void Despawn()
    {
        base.Despawn();
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
        this.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!IsPlaying())
        {
            if (agent != null && agent.isActiveAndEnabled && agent.isOnNavMesh && !agent.isStopped)
            {
                agent.isStopped = true;
                agent.velocity = Vector3.zero;
            }
            return;
        }
        if (IsPlaying())
        {
            if (agent != null && agent.isStopped)
            {
                agent.isStopped = false;
            }
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
        return stage.GetNearestBridge(this);
    }

    public bool ReachedDestination()
    {
        return !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance && (!agent.hasPath || agent.velocity.sqrMagnitude == 0f);
    }

    public int GetStairWalkable(int brickCount)
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
    public int GetMaxBrick()
    {
        return maxBrick;
    }
    [ContextMenu("Show Destination")]
    public void ShowDestination()
    {
        Debug.Log("Show Destination: Transform: " + transform.position + ",Destibation: " + agent.destination);
    }
}
