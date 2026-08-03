using System.Collections;
using System.Collections.Generic;
using MyNamespace;
using UnityEngine;

public class Bridge : MonoBehaviour
{
    [SerializeField] private List<Stair> stairs = new List<Stair>();
    [SerializeField] private Door door;
    private Stage stage;
    private Transform tf;
    
    public void OnInit(Stage stage)
    {
        foreach (Stair stair in stairs)
        {
            stair.OnInit(stage);
        }
        door.OnInit(stage,this);
    }
    public void Despawn()
    {
        foreach (Stair stair in stairs)
        {
            stair.Despawn();
        }
        door.Despawn();
    }

    // kiem tra xem co the mo cua ko
    public bool CanOpenDoor()
    {
        if (stairs == null || stairs.Count == 0)
        {
            Debug.LogError("Can't open door without stairs");
            return false;
        }
        for (int i = 0; i < stairs.Count; i++)
        {
            if (!stairs[i].HasFilled()) return false;
        }
        return true;
    }

    // kiem tra stair cao nhat duoc fill chua
    public bool IsFilledHighestStair(ColorType colorType)
    {
        return colorType == stairs[stairs.Count - 1].GetColorType();
    }
    // kiem tra xem co duoc di qua cau ko
    public bool CanCrossBridge(int stairWalkeableCount)
    {
        int numStairs = stairs.Count;
        return stairWalkeableCount == numStairs;
    }
    
    // lay so stair co the di
    public int GetStairWalkable(ColorType color, int brickCount)
    {
        int stairWalkable = 0;
        foreach (Stair stair in stairs)
        {
            if (stair.GetColorType() == color)
            {
                stairWalkable++;
            }
            if (stair.GetColorType() != color)
            {
                if (brickCount >= 1)
                {
                    brickCount--;
                    stairWalkable++;
                }
                else
                {
                    break;
                }
            }
        }

        if (stairWalkable >= stairs.Count)
        {
            stairWalkable = stairs.Count;
        }
        return stairWalkable;
    }

    public Transform GetTransform()
    {
        if (tf == null)
        {
            tf = transform;
        }
        return tf;
    }

    public List<Stair> GetStairs()
    {
        return stairs;
    }
}
