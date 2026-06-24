using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class JoyStick : MonoBehaviour, IDragHandler
{
    [SerializeField] private RectTransform BG;
    [SerializeField] private RectTransform handle;
    [SerializeField] private float handleSize = 1f;
    
    public void OnDrag(PointerEventData eventData)
    {
        
    }
}
