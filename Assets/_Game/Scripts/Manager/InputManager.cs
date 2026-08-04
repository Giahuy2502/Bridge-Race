using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    [SerializeField] private JoyStick joyStick;
    [SerializeField] private Vector2 moveDirection;
    
    private UIController UIController => UIController.Instance;
    public void OnInit()
    {
        joyStick = UIController.GetCanvasInput().GetJoyStick();
    }

    // ham tra ve move direction
    public Vector2 GetMoveDirection()
    {
        if (joyStick == null)
        {
            Debug.LogError("JoyStick is null");
            return Vector2.zero;
        }
        moveDirection = joyStick.GetDirection();
        return moveDirection;
    }

    // ham nay de tat input
    public void Despawn()
    {
        joyStick = null;
    }
}
