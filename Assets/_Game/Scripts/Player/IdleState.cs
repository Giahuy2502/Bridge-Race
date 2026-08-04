using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class IdleState : IState
{
    public void OnEnter(Bot bot)
    {
        bot.SetTimer(0f);
        bot.StopMove();
    }

    public void OnExcute(Bot bot)
    {
        bot.AddToTimer(Time.deltaTime);
        if (bot.GetTimer() >= bot.GetIdleTime())
        {
            bot.ChangeState(new PatrolState());
        }
    }

    public void OnExit(Bot bot)
    {
    }
}
