using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishPlatform : MonoBehaviour
{
    [SerializeField] private List<Transform> finishPositions = new List<Transform>();
    private RankManager RankManager => RankManager.Instance;
    public void OnInit()
    {
        RankManager.SetFinishedPosition(finishPositions);
    }
}
