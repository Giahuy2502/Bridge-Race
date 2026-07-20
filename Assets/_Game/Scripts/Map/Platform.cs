using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] private Stage stage;
    [SerializeField] private List<Bridge> bridges;
    [SerializeField] private GameObject baries;
    public void OnInit()
    {
        // khoi tao stage
        if (stage == null)
        {
            Debug.LogError("No stage selected");
            return;
        }
        stage.OnInit();
        
        // khoi tao bridge
        if (bridges == null || bridges.Count == 0)
        {
            Debug.LogError("No bridges selected");
            return;
        }

        foreach (Bridge bridge in bridges)
        {
            bridge.OnInit(stage);
        }
        // khoi tao baries
    }
    public void Despawn()
    {
        if (stage == null)
        {
            Debug.LogError("No stage selected");
            return;
        }
        stage.Despawn();
    }
}
