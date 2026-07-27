using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasInput : UICanvas
{
    [SerializeField] private JoyStick joyStick;
    
    public JoyStick JoyStick=> joyStick;
    
}
