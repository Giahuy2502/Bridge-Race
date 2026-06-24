using System;
using System.Collections;
using System.Collections.Generic;
using MyNamespace;
using UnityEngine;

public class Brick : GameUnit
{
    [SerializeField] Renderer renderer;
    [SerializeField] ColorDataSO colorDataSO;
    [SerializeField] private float speed = 5f;
    [SerializeField] private TrailRenderer trailRenderer;
    private Vector3 startPosition;
    private Stage stage;
    private bool isTaked = false;
    public ColorType ColorType { get; private set;}
    public Stage Stage { get => stage;set => stage = value; }
    public void OnInit(ColorType color)
    {
        ChangeColor(color);
        startPosition = transform.position;
        isTaked = false;
        trailRenderer.enabled = false;
        trailRenderer.emitting = false;
    }

    public void Despawn()
    {
        ChangeColor(ColorType.None);
        transform.SetParent(null);
        transform.position = startPosition;
        if(stage != null) stage.Despawn(this);
        SimplePool.Despawn(this);
    }

    public void ChangeColor(ColorType colorType)
    {
        this.ColorType = colorType;
        renderer.material = colorDataSO.GetMat(colorType);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!isTaked && (other.CompareTag(Variables.PLAYER_TAG)||other.CompareTag(Variables.BOT_TAG)))
        {
            Character character = MyCache.GetCharacter<Character>(other);
            if (ColorType != character.ColorType) return;
            MoveBrick(character.BricksTF, character);
            isTaked = true;
        }
        
    }

    public void MoveBrick(Transform characterBrick, Character character)
    {
        transform.SetParent(characterBrick);
        Vector3 newPos = character.GetNewestBrickPos();
        StartCoroutine(MoveBrickToNewPos(newPos,character));
    }

    IEnumerator MoveBrickToNewPos(Vector3 newPos, Character character)
    {
        trailRenderer.enabled = true;
        trailRenderer.emitting = true;
        trailRenderer.Clear();
        while (Vector3.Distance(transform.localPosition, newPos) > 0.1f)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, newPos, Time.deltaTime * speed);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.identity, Time.deltaTime * speed);
            yield return null;
        }
        transform.localPosition = newPos;
        transform.localRotation = Quaternion.identity;
        trailRenderer.enabled = false;
        trailRenderer.emitting = false;
        Despawn();
        character.AddBrick();
    }
}
