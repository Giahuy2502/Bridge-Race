using System;
using System.Collections;
using System.Collections.Generic;
using MyNamespace;
using Unity.VisualScripting;
using UnityEngine;
using Variables = MyNamespace.Variables;

public class Player : Character
{
    [SerializeField] private Camera camera;
    [SerializeField] private float movementSpeed;
    [SerializeField] private LayerMask groundLayer;
    private Ray ray;
    private RaycastHit raycastHit;
    private Vector3 targerPos;

    private void Start()
    {
        base.Start();
        targerPos = tf.position;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out raycastHit, Mathf.Infinity, groundLayer))
            {
                targerPos = raycastHit.point;
            }
        }
        Move(targerPos);
    }

    public override void Move(Vector3 pos)
    {
        if (Vector3.Distance(tf.position, pos) < 0.1f)
        {
            ChangeAnim(Variables.IDLE_ANIM);
            return;
        }
        tf.position = Vector3.MoveTowards(tf.position, pos, Time.deltaTime * movementSpeed);
        base.Move(pos);
        ChangeAnim(Variables.RUN_ANIM);
    }

    public override void RotateToTarget(Vector3 pos)
    {
        Vector3 direction = pos - transform.position;
        direction.y = 0;

        tf.rotation = Quaternion.LookRotation(direction);
    }
}
