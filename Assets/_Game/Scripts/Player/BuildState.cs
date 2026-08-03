using System.Collections;
using System.Collections.Generic;
using UnityEditor.XR;
using UnityEngine;

public class BuildState : IState
{
    private Bridge nearestBridge;
    private Vector3 targetPos;
    private int indexStair = 0;
    private bool isApproachingBridge = true;
    private bool isCrossing = false;
    private Vector3 highestStairPos;
    private int stairWalkeableCount = 0;
    public void OnEnter(Bot bot)
    {
        isApproachingBridge = true;
        isCrossing = false;
        stairWalkeableCount = 0;
        nearestBridge = bot.GetNearestBridge();
        targetPos = nearestBridge.GetStairs()[0].transform.position + Vector3.back*1.15f;
        bot.SetDestination(targetPos);
    }

    public void OnExcute(Bot bot)
    {
        if (bot.Bricks.Count <= 0 && !nearestBridge.IsFilledHighestStair(bot.ColorType))
        {
            bot.ChangeState(new PatrolState());
            return;
        } 
        if (bot.ReachedDestination())
        {
            if (isApproachingBridge)
            {
                isApproachingBridge = false;
                highestStairPos = GetHighestStairPos(bot);
                bot.SetDestination(highestStairPos);
                bot.SetBuildState(true);
                return;
            }
            if(isCrossing)
            {
                bot.ChangeState(new PatrolState());
                return;
            }
            if(!isApproachingBridge && !isCrossing)
            {
                if (nearestBridge == null)
                {
                    Debug.LogError("Nearest Bridge not found");
                    return;
                }
                if (nearestBridge.CanCrossBridge(stairWalkeableCount))
                {
                    highestStairPos = bot.transform.position + Vector3.forward * 5;
                    bot.SetDestination(highestStairPos);
                    isCrossing = true;
                }
                else
                {
                    bot.ChangeState(new PatrolState());
                }
            }
        }
    }

    public void OnExit(Bot bot)
    {
        bot.Agent.velocity = Vector3.zero;
        bot.SetBuildState(false);
    }

    private bool ValidIndex(int index)
    {
        if (nearestBridge == null)
        {
            return false;
        }
        return index >= 0 && index < nearestBridge.GetStairs().Count;
    }
    
    private Vector3 GetHighestStairPos(Bot bot)
    {
        stairWalkeableCount = bot.GetStairWalkable(bot.Bricks.Count);
        if (stairWalkeableCount <= 0)
        {
            bot.StopMove();
            return bot.transform.position;
        }
        indexStair = stairWalkeableCount - 1;
        if (!ValidIndex(indexStair))
        {
            bot.StopMove();
        }
        return nearestBridge.GetStairs()[indexStair].transform.position + Vector3.up*0.25f;
    }
}
