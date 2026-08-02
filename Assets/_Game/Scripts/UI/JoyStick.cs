using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private RectTransform BG;
    [SerializeField] private RectTransform handle;
    [SerializeField] private List<GameObject> moveFocus = new List<GameObject>();
    [SerializeField] private float handleRange = 1f;

    private Vector2 inputDirection = Vector2.zero;
    private Vector2 position = Vector2.zero;
    public Vector2 Direction => inputDirection;

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }
    public void OnDrag(PointerEventData eventData)
    {
        // Chuyển đổi vị trí chạm màn hình sang local position của RectTransform BG
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(BG, eventData.position, eventData.pressEventCamera, out position))
        {
            // Chia cho kích thước của BG để đưa về khoảng từ -0.5 đến 0.5 (hoặc tỷ lệ chuẩn)
            position.x = (position.x / BG.sizeDelta.x);
            position.y = (position.y / BG.sizeDelta.y);
            inputDirection = new Vector2(position.x * 2, position.y * 2);
            // Giới hạn hướng trong một hình tròn bán kính = 1
            inputDirection = (inputDirection.magnitude > 1.0f) ? inputDirection.normalized : inputDirection;
            // Di chuyển nút handle theo hướng đã tính nhân với bán kính cho phép
            handle.anchoredPosition = new Vector2(
                inputDirection.x * (BG.sizeDelta.x / 2) * handleRange,
                inputDirection.y * (BG.sizeDelta.y / 2) * handleRange
            );
        }
        // active move focus
        ActiveMoveFocus(GetVectorIndexMoveFocus(inputDirection));
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        ResetState();
    }

    public void ResetState()
    {
        inputDirection = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
        DisableMoveFocus();
    }
    public Vector2Int GetVectorIndexMoveFocus(Vector2 inputDirection)
    {
        Vector2Int vector = Vector2Int.zero;
        if (inputDirection == Vector2.zero)
        {
            vector = new Vector2Int(-1, -1);
            return vector;
        }
        vector.x = (inputDirection.x <= 0) ? 0 : 1;
        vector.y = (inputDirection.y <= 0) ? 0 : 1;
        return vector;
    }
    public void ActiveMoveFocus(Vector2Int vectorIndex)
    {
        DisableMoveFocus();
        if (vectorIndex == new Vector2Int(-1, -1))
        {
            return;
        }

        if (vectorIndex == Vector2Int.zero)
        {
            moveFocus[2].SetActive(true);
            return;
        }
        else if (vectorIndex == Vector2Int.one)
        {
            moveFocus[1].SetActive(true);
        }
        else if (vectorIndex == Vector2Int.up)
        {
            moveFocus[0].SetActive(true);
        }
        else if (vectorIndex == Vector2Int.right)
        {
            moveFocus[3].SetActive(true);
        }
    }
    private void DisableMoveFocus()
    {
        foreach (GameObject image in moveFocus)
        {
            image.SetActive(false);
        }
    }
}
