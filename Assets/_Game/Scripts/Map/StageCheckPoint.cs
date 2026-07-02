using System;
using System.Collections;
using System.Collections.Generic;
using MyNamespace;
using UnityEngine;

public class StageCheckPoint : MonoBehaviour
{
    [SerializeField] private Stage stage;
    private RankManager RankManager => RankManager.Instance;
    public void OnTriggerEnter(Collider other)
    {
        Character character = MyCache.GetCharacter<Character>(other);
        if (character != null)
        {
            RankManager.RegisterStagePassed(character, stage);
        }
    }
}
