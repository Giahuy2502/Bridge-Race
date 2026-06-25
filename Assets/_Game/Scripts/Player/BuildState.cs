using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildState : IState
{
    private Bridge nearestBridge;
    private Vector3 targetPos;
    private int indexStair = 0;
    private bool isMoving = false;
    private Vector3 endPos;
    public void OnEnter(Bot bot)
    {
        isMoving = false;
        nearestBridge = bot.GetNearestBridge();
        targetPos = nearestBridge.Stairs[0].transform.position + Vector3.back*1.15f;
        bot.SetDestination(targetPos);
    }

    public void OnExcute(Bot bot)
    {
        // di chuyen den stair gan nhat
        if (bot.ReachedDestination())
        {
            if (Vector3.Distance(bot.transform.position, endPos) <= 0.4f)
            {
                bot.ChangeState(new PatrolState());
            }
            else
            {
                endPos = GetEndPos(bot);
                bot.SetDestination(endPos);
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
    
    private Vector3 GetEndPos(Bot bot)
    {
        int stairWalkeableCount = bot.GetStairWalkable(bot.Bricks.Count);
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
