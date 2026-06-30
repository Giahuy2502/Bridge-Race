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
    [SerializeField] private TrailRenderer[] trailRenderers;
    [SerializeField] private Vector3 startPosition;
    [SerializeField] private Stage stage;
    private bool isTaked = false;
    public ColorType ColorType { get; private set;}
    public Stage Stage { get => stage;set => stage = value; }
    public Vector3 StartPosition { get => startPosition; set => startPosition = value; }
    public void OnInit(ColorType color,Stage stage = null)
    {
        ChangeColor(color);
        ChangeTrailRendererColor(color,trailRenderers);
        if (stage != null)
        {
            this.Stage = stage;
        }
        isTaked = false;
        TurnOffTrailRenderer(trailRenderers);
        this.name = "Brick "+ColorType.ToString();
    }

    public void Despawn()
    {
        transform.SetParent(null);
        // Debug.Log("Despawn brick" + TF.position +" "+ startPosition);
        transform.position = startPosition;
        transform.rotation = Quaternion.identity;
        // Debug.Log("Despawn brick" + TF.position +" "+ startPosition);
        this.gameObject.SetActive(false);
    }

    private void ChangeColor(ColorType colorType)
    {
        this.ColorType = colorType;
        renderer.material = colorDataSO.GetMat(colorType);
    }

    private void ChangeTrailRendererColor(ColorType colorType, TrailRenderer[] trailRenderers)
    {
        Material material = colorDataSO.GetMat(colorType);
        Gradient gradient = new Gradient();
        GradientColorKey[] colorKeys = new GradientColorKey[2];
        colorKeys[0] = new GradientColorKey(material.color, 0.0f);
        colorKeys[1] = new GradientColorKey(material.color, 1.0f);
        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
        alphaKeys[0] = new GradientAlphaKey(material.color.a, 0.0f);
        alphaKeys[1] = new GradientAlphaKey(0f, 1.0f);
        gradient.SetKeys(colorKeys, alphaKeys);
        foreach (TrailRenderer trailRenderer in trailRenderers)
        {
            trailRenderer.colorGradient = gradient;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!isTaked && (other.CompareTag(Variables.PLAYER_TAG)||other.CompareTag(Variables.BOT_TAG)))
        {
            Character character = MyCache.GetCharacter<Character>(other);
            if (ColorType != character.ColorType) return;
            if(stage != null) stage.DespawnBrick(this);
            MoveBrick(character.BricksTF, character);
            isTaked = true;
        }
        
    }

    private void MoveBrick(Transform characterBrick, Character character)
    {
        transform.SetParent(characterBrick);
        Vector3 newPos = character.GetNewestBrickPos();
        StartCoroutine(MoveBrickToNewPos(newPos,character));
    }
    IEnumerator MoveBrickToNewPos(Vector3 newPos, Character character)
    {
        TurnOnTrailRenderer(trailRenderers);
        yield return StartCoroutine(MoveToBackCharacter(newPos - Vector3.forward));
        while (Vector3.Distance(transform.localPosition, newPos) > 0.1f)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, newPos, Time.deltaTime * speed);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.identity, Time.deltaTime * speed);
            yield return null;
        }
        transform.localPosition = newPos;
        transform.localRotation = Quaternion.identity;
        TurnOffTrailRenderer(trailRenderers);
        Despawn();
        character.AddBrick();
    }
    IEnumerator MoveToBackCharacter(Vector3 backPos)
    {
        while (Vector3.Distance(transform.localPosition, backPos) > 0.1f)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, backPos, Time.deltaTime * speed);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.identity, Time.deltaTime * speed);
            yield return null;
        }
    }
    private void TurnOnTrailRenderer(TrailRenderer[] trailRenderers)
    {
        foreach (TrailRenderer trailRenderer in trailRenderers)
        {
            trailRenderer.enabled = true;
            trailRenderer.emitting = true;
            trailRenderer.Clear();
        }
    }
    private void TurnOffTrailRenderer(TrailRenderer[] trailRenderers)
    {
        foreach (TrailRenderer trailRenderer in trailRenderers)
        {
            trailRenderer.enabled = false;
            trailRenderer.emitting = false;
        }
    }

    public void SetStartPosition(Vector3 position)
    {
        startPosition = position;
        TF.position = startPosition;
    }
}
