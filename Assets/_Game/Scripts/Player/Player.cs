using System;
using System.Collections;
using System.Collections.Generic;
using MyNamespace;
using Unity.VisualScripting;
using UnityEngine;
using Variables = MyNamespace.Variables;

public class Player : Character
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationSpeed; 
    [SerializeField] private LayerMask stairLayer;
    [SerializeField] private float deadY = -10f; // neu y cua player < deady => roi khoi map
    private Ray ray;
    private RaycastHit raycastHit;
    private Vector3 targerPos;
    private float blockPosY;
    private Vector2 direction;
    private InputManager InputManager => InputManager.Instance;
    
    public override void OnInit(ColorType colorType)
    {
        base.OnInit(colorType);
        targerPos = tf.position;
        blockPosY = tf.position.y;
    }

    public override void Despawn()
    {
        base.Despawn();
        this.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!IsPlaying()) return;
        if (!IsOnGround())
        {
            SetStartPoints();
        };
        direction = InputManager.GetMoveDirection();
        float moveX = direction.x;
        float moveY = direction.y;
        Vector3 movement = new Vector3(moveX, 0, moveY);
        Move(movement);
    }
    // ham di chuyen nhan vat
    private void Move(Vector3 movement)
    {
        if (movement.magnitude <= 0.1f)
        {
            ChangeAnim(Variables.IDLE_ANIM);
            return;
        }
        ChangeAnim(Variables.RUN_ANIM);
        RotateToMoveDirection(movement);
        if (IsBlockByStair(movement) || IsBlockByDoor(movement))
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

    private void RotateToMoveDirection(Vector3 movement)
    {
        tf.rotation = Quaternion.Lerp(tf.rotation, Quaternion.LookRotation(movement.normalized), Time.deltaTime * rotationSpeed);
    }
    
    // kiem tra co bi chan boi stair
    private bool IsBlockByStair(Vector3 movement)     
    {
        if (movement.z <= 0)
        {
            blockPosY = tf.position.y;
            return false;
        }
        Vector3 ray = tf.position + Vector3.up * 0.3f + Vector3.forward * 1f;
        // Debug.DrawRay(ray, Vector3.down * 2f, Color.red);
        RaycastHit hit;
        if (Physics.Raycast(ray, Vector3.down, out hit, 2.5f, stairLayer))
        {
            Stair stair = MyCache.GetStair<Stair>(hit.collider);
            if (stair != null)
            {
                float newY = stair.transform.position.y - 0.15f;
                if (stair.CheckCanBlockPlayer(this) && Math.Abs(blockPosY- newY) >0.2f)
                {
                    blockPosY = newY;
                }
                return stair.CheckCanBlockPlayer(this);
            }
        }
        return false;
    }
    // kiem tra co bi chan boi door
    private bool IsBlockByDoor(Vector3 movement)
    {
        if (movement.z >= 0)
        {
            blockPosY = tf.position.y;
            return false;
        }
        Vector3 ray = tf.position + Vector3.up * 0.5f;
        Vector3 rayDirection = movement.normalized;
        RaycastHit hit;
        if (Physics.Raycast(ray , rayDirection, out hit, 0.75f, stairLayer))
        {
            if (hit.transform.CompareTag(Variables.DOOR_TAG))
            {
                return true;
            }
        }
        return false;
    }

    private bool IsOnGround()
    {
        if (tf.position.y >= deadY)
        {
            return true;
        }
        return false;
    }

    private void SetStartPoints()
    {
        Despawn();
        OnInit(this.colorType);
        SetStartPoints(GetStartPos());
    }

    public override void AddBrick()
    {
        base.AddBrick();
        PlaySFX(FxID.SFX_CollectBrick);
    }

    public override void RemoveBrick()
    {
        base.RemoveBrick();
        PlaySFX(FxID.SFX_BuildBridge);
    }
}
