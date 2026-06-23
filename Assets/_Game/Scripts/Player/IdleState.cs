using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class IdleState : IState
{
    private float time = 0;
    private float idleTime = 1f;
    public void OnEnter(Bot bot)
    {
        time = 0f;
        bot.StopMove();
    }

    public void OnExcute(Bot bot)
    {
        time += Time.deltaTime;
        if (time >= idleTime)
        {
            bot.ChangeState(new PatrolState());
        }
    }

    public void OnExit(Bot bot)
    {
    }
}
