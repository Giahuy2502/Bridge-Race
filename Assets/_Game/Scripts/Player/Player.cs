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
    [SerializeField] private float rotationSpeed; 
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
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(moveX, 0, moveY);
        Move(movement);
    }

    public void Move(Vector3 movement)
    {
        if (movement.magnitude <= 0.1f)
        {
            ChangeAnim(Variables.IDLE_ANIM);
            return;
        }
        targerPos = tf.position + movement;
        tf.position = Vector3.MoveTowards(tf.position, targerPos, Time.deltaTime * movementSpeed);
        ChangeAnim(Variables.RUN_ANIM);
        RotateToTarget(movement);
    }

    public void RotateToTarget(Vector3 movement)
    {
        tf.rotation = Quaternion.Lerp(tf.rotation, Quaternion.LookRotation(movement), Time.deltaTime * rotationSpeed);
    }
}
