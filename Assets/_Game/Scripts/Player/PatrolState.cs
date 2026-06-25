using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

public class PatrolState : IState
{
    public void OnEnter(Bot bot)
    {
        if(bot.Bricks.Count < bot.MaxBrick)
        {
            Brick brick = bot.GetNearestBrick();
            if (brick != null)
            {
                bot.SetDestination(bot.GetNearestBrickPos(brick));
            }
            else
            {
                bot.StopMove();
            }
        }
        else
        {
            bot.ChangeState(new BuildState());
        }
    }

    public void OnExcute(Bot bot)
    {
        if (bot.ReachedDestination())
        {
            bot.StopMove();
            if(bot.Bricks.Count < bot.MaxBrick)
            {
                Brick brick = bot.GetNearestBrick();
                if (brick != null)
                {
                    bot.SetDestination(bot.GetNearestBrickPos(brick));
                }
                else
                {
                    if (bot.Bricks.Count > 0)
                    {
                        bot.ChangeState(new BuildState());
                    }
                    else
                    {
                        bot.StopMove();
                    }
                }
            }
            else
            {
                bot.ChangeState(new BuildState());
            }
        }
    }

    public void OnExit(Bot bot)
    {
        bot.StopMove();
    }
}
