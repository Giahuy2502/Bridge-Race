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
    [SerializeField] private LayerMask stairLayer;
    private Ray ray;
    private RaycastHit raycastHit;
    private Vector3 targerPos;
    private float blockPosY;

    private void Start()
    {
        base.Start();
        targerPos = tf.position;
        blockPosY = tf.position.y;
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
        ChangeAnim(Variables.RUN_ANIM);
        RotateToTarget(movement);
        if (IsBlockByStair(movement))
        {
            if (Math.Abs(blockPosY - tf.position.y) > 0.2f)
            {
                tf.position = new Vector3(tf.position.x, blockPosY, tf.position.z);
            }
            return;
        }
        targerPos = tf.position + movement;
        tf.position = Vector3.MoveTowards(tf.position, targerPos, Time.deltaTime * movementSpeed);
    }

    public void RotateToTarget(Vector3 movement)
    {
        tf.rotation = Quaternion.Lerp(tf.rotation, Quaternion.LookRotation(movement), Time.deltaTime * rotationSpeed);
    }

    private bool IsBlockByStair(Vector3 movement)
    {
        if (movement.z <= 0)
        {
            blockPosY = tf.position.y;
            return false;
        }

        Vector3 ray = tf.position + Vector3.up * 0.3f + Vector3.forward * 1f;
        Debug.DrawRay(ray, Vector3.down * 2f, Color.red);
        RaycastHit hit;
        if (Physics.Raycast(ray, Vector3.down, out hit, 2.5f, stairLayer))
        {
            Stair stair = hit.collider.GetComponent<Stair>();
            if (stair != null)
            {
                float newY = stair.transform.position.y - 0.15f;
                if (stair.CheckCanBlockCharacter(this) && Math.Abs(blockPosY- newY) >0.2f)
                {
                    blockPosY = newY;
                }
                return stair.CheckCanBlockCharacter(this);
            }
        }
        return false;
    }
}
