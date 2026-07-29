using System;
using System.Collections;
using System.Collections.Generic;
using MyNamespace;
using UnityEngine;

public class FinishBox : MonoBehaviour
{
    private GameManager GameManager => GameManager.Instance;
    private RankManager RankManager => RankManager.Instance;
    public void OnTriggerEnter(Collider other)
    {
        Character character = MyCache.GetCharacter<Character>(other);
        if (character != null)
        {
            RankManager.SetWinner(character);
            GameManager.OnEndGame();
        }
    }

    public void RanksCharacter()
    {
        
    }
}
