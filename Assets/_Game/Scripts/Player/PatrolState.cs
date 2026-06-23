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
            bot.SetDestination();
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
                bot.SetDestination();
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
