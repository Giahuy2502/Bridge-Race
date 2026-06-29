using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildState : IState
{
    private Bridge nearestBridge;
    private Vector3 targetPos;
    private int indexStair = 0;
    private bool isMoving = false;
    private Vector3 highestStairPos;
    private int stairWalkeableCount = 0;
    public void OnEnter(Bot bot)
    {
        isMoving = false;
        stairWalkeableCount = 0;
        nearestBridge = bot.GetNearestBridge();
        targetPos = nearestBridge.Stairs[0].transform.position + Vector3.back*1.15f;
        bot.SetDestination(targetPos);
    }

    public void OnExcute(Bot bot)
    {
        // di chuyen den stair gan nhat
        if (bot.ReachedDestination())
        {
            // di den bac thang cao nhat
            if (Vector3.Distance(bot.transform.position, highestStairPos) <= 0.4f)
            {
                // Debug.Log("Reached the highest stair");
                nearestBridge = bot.GetNearestBridge();
                if (nearestBridge.CanCrossBridge(stairWalkeableCount))
                {
                    bot.SetDestination(bot.transform.position + Vector3.forward * 5);
                    // Debug.Log("Can Cross Bridge"+ stairWalkeableCount +" "+ nearestBridge.Stairs.Count);
                }
                else
                {
                    bot.ChangeState(new PatrolState());
                    // Debug.Log("Can't Cross Bridge " + stairWalkeableCount +" "+ nearestBridge.Stairs.Count);
                }
            }
            // di chuyen den truoc Bridge
            else
            {
                highestStairPos = GetHighestStairPos(bot);
                bot.SetDestination(highestStairPos);
            }
        }
    }

    public void OnExit(Bot bot)
    {
        // neu di qua bridge thi chuyen sang patrol state o stage moi
        
        // neu het brick thi chuyen sang patrol state va chon brick o tang duoi
        
    }

    private bool ValidIndex(int index)
    {
        if (nearestBridge == null)
        {
            return false;
        }
        return index >= 0 && index < nearestBridge.Stairs.Count;
    }
    
    private Vector3 GetHighestStairPos(Bot bot)
    {
        stairWalkeableCount = bot.GetStairWalkable(bot.Bricks.Count);
        if (stairWalkeableCount == 0)
        {
            bot.StopMove();
            return bot.transform.position;
        }
        indexStair = stairWalkeableCount - 1;
        if (!ValidIndex(indexStair))
        {
            bot.StopMove();
        }
        return nearestBridge.Stairs[indexStair].transform.position + Vector3.up*0.25f;
    }
}
