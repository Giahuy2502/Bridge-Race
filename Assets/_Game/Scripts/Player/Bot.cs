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

    public override void OnInit()
    {
        base.OnInit();
        this.name = "Bot-"+ColorType.ToString();
        ChangeState(new IdleState());
    }

    private void Update()
    {
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
}
