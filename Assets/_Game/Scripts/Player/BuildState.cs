using System.Collections;
using System.Collections.Generic;
using UnityEditor.XR;
using UnityEngine;

public class BuildState : IState
{
    
    public void OnEnter(Bot bot)
    {
        bot.SetIsApproachingBridge(true);
        bot.SetIsCrossing(false);
        bot.SetStairWalkeableCount(0);
        Bridge nearestBridge = bot.GetNearestBridge();
        Vector3 targetPos = nearestBridge.GetStairs()[0].transform.position + Vector3.back*1.15f;
        bot.SetDestination(targetPos);
    }

    public void OnExcute(Bot bot)
    {
        if (bot.GetListBricksCount() <= 0 && !bot.GetNearestBridge().IsFilledHighestStair(bot.GetColorType()))
        {
            bot.ChangeState(new PatrolState());
            return;
        } 
        if (bot.IsReachedDestination())
        {
            if (bot.IsApproachingBridge())
            {
                bot.SetIsApproachingBridge(false);
                bot.SetHighestStairPos();
                bot.SetDestination(bot.GetHighestStairPos());
                bot.SetBuildState(true);
                return;
            }
            if(bot.IsCrossing())
            {
                bot.ChangeState(new PatrolState());
                return;
            }
            if(!bot.IsApproachingBridge() && !bot.IsCrossing())
            {
                if (bot.NearestBridge() == null)
                {
                    Debug.LogError("Nearest Bridge not found");
                    return;
                }
                if (bot.GetNearestBridge().CanCrossBridge(bot.GetStairWalkeableCount()))
                {
                    bot.SetHighestStairPos(bot.transform.position + Vector3.forward * 5);
                    bot.SetDestination(bot.GetHighestStairPos());
                    bot.SetIsCrossing(true);
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
        bot.StopMove();
        bot.SetBuildState(false);
    }
}
