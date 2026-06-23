using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildState : IState
{
    public void OnEnter(Bot bot)
    {
        bot.StopMove();
    }

    public void OnExcute(Bot bot)
    {
    }

    public void OnExit(Bot bot)
    {
    }
}
