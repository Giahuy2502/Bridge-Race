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
    private ColorType color;
    public void OnInit(ColorType color,Stage stage = null)
    {
        ChangeColor(color);
        ChangeTrailRendererColor(color,trailRenderers);
        if (stage != null)
        {
            this.stage = stage;
        }
        isTaked = false;
        TurnOffTrailRenderer(trailRenderers);
        this.name = "Brick "+color.ToString();
    }
    public void Despawn()
    {
        transform.SetParent(null);
        transform.position = startPosition;
        transform.rotation = Quaternion.identity;
        this.gameObject.SetActive(false);
        if(stage != null) stage.DespawnBrick(this);
    }
    // ham doi mau brick
    private void ChangeColor(ColorType colorType)
    {
        this.color = colorType;
        renderer.material = colorDataSO.GetMat(colorType);
    }
    // ham chuyen mau trail renderer
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
            if (color != character.GetColorType()) return;
            isTaked = true;
            MoveBrick(character.GetBricksTF(), character);
        }
    }
    // ham di chuyen brick toi nhan vat
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
    
    // di chuyen brick ra sau nhan vat
    IEnumerator MoveToBackCharacter(Vector3 backPos)
    {
        while (Vector3.Distance(transform.localPosition, backPos) > 0.1f)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, backPos, Time.deltaTime * speed);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.identity, Time.deltaTime * speed);
            yield return null;
        }
    }
    // ham bat trail renderer
    private void TurnOnTrailRenderer(TrailRenderer[] trailRenderers)
    {
        foreach (TrailRenderer trailRenderer in trailRenderers)
        {
            trailRenderer.enabled = true;
            trailRenderer.emitting = true;
            trailRenderer.Clear();
        }
    }
    // ham tat trail renderer
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
    public ColorType GetColor()
    {
        return color;
    }

    public Vector3 GetStartPosition()
    {
        return startPosition;
    }
}
