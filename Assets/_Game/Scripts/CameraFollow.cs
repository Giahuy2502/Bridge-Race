using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Transform endCamTF;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float speed;
    [SerializeField] private float transitionDuration = 1.5f;
    private bool isEndGame;
    private float transitionTime;
    private void LateUpdate()
    {
        if (isEndGame)
        {
            MoveToEndGameCamera();
        }
        else
        {
            MoveToTarget();
        }
    }
    // di chuyen theo nhan vat
    private void MoveToTarget()
    {
        transform.position = Vector3.Lerp(transform.position, target.position + offset, Time.deltaTime* speed);
    }
    // di chuyen den end game camera 
    private void MoveToEndGameCamera()
    {
        if (endCamTF == null)
        {
            return;
        }
        transitionTime += Time.deltaTime;
        float t = Mathf.Clamp01(transitionTime / transitionDuration);
        transform.position = Vector3.Lerp(transform.position, endCamTF.position, t);
    }

    public void SetEndGame(bool isEndGame)
    {
        this.isEndGame = isEndGame;
        transitionTime = 0f;
    }

    public void SetEndCamTF(Transform endCamTF)
    {
        this.endCamTF = endCamTF;
    }
}
