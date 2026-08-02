using System.Collections;
using System.Collections.Generic;
using MyNamespace;
using TMPro;
using UnityEngine;

public class CountDown : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countDownText;
    [SerializeField] private Animator countDownAnimator;
    [SerializeField] private float countDownDuration = 3.1f;

    GameManager GameManager => GameManager.Instance;
    public void OnInit()
    {
        this.gameObject.SetActive(true);
        Invoke(nameof(Despawn), countDownDuration);
        PlayCountDownAnimation();
    }
    public void PlayCountDownAnimation()
    {
        countDownText.gameObject.SetActive(true);
    }

    public void UpdateCountDownText(string text)
    {
        Debug.Log("Animation Event: " + text);
        countDownText.text = text;
    }

    void Despawn()
    {
        this.gameObject.SetActive(false);
    }

    public void OnCountDownFinished()
    {
        GameManager.ChangeStateOnCountDown();
    }
}
