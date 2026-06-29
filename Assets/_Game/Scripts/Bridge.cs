using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour
{
    [SerializeField] private List<Stair> stairs = new List<Stair>();

    private Transform tf;
    public Transform TF
    {
        get
        {
            if (tf == null)
            {
                tf = transform;
            }
            return tf;
        }
    }
    public List<Stair> Stairs
    {
        get
        {
            return stairs;
        }
        private set{stairs = value;}
    }
    
    public bool CanCrossBridge(int stairWalkeableCount)
    {
        int numStairs = Stairs.Count;
        return stairWalkeableCount == numStairs;
    }
}
