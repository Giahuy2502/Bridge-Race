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
    public int MaxBrick{get{return maxBrick;}}
    public NavMeshAgent Agent{get{return agent;}}

    public override void OnInit(Transform startPoints)
    {
        base.OnInit(startPoints);
        this.name = "Bot-"+ColorType.ToString();
        agent.enabled = true;
        ChangeState(new IdleState());
    }

    private void Update()
    {
        if (!IsPlaying()) return;
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
        ChangeAnim(Variables.IDLE_ANIM);
    }

    public Brick GetNearestBrick()
    {
        return Stage.GetNearestBrick(this);
    }

    public Vector3 GetNearestBrickPos(Brick brick)
    {
        if(brick == null) return Vector3.zero;
        return brick.transform.position;
    }

    public Bridge GetNearestBridge()
    {
        return Stage.GetNearestBridge(this);
    }

    public bool ReachedDestination()
    {
        return !agent.pathPending &&
               agent.remainingDistance <= agent.stoppingDistance &&
               (!agent.hasPath || agent.velocity.sqrMagnitude == 0f);
    }

    public int GetStairWalkable(int brickCount)
    {
        return Stage.GetStairWalkable(this.ColorType, brickCount,Stage.GetNearestBridge(this));
    }

    public override void SetWinState()
    {
        base.SetWinState();
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
        StopMove();
    }
    
    [ContextMenu("Show Destination")]
    public void ShowDestination()
    {
        Debug.Log("Show Destination: Transform: " + transform.position + ",Destibation: " + agent.destination);
    }
}
