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
    private GameManager GameManager => GameManager.Instance;
    public void OnInit()
    {
        this.gameObject.SetActive(true);
        Invoke(nameof(Despawn), countDownDuration);
        PlayCountDownAnimation();
    }
    // chay count down
    private void PlayCountDownAnimation()
    {
        countDownText.gameObject.SetActive(true);
    }
    // cap nhat count down text
    public void UpdateCountDownText(string text)
    {
        countDownText.text = text;
    }

    void Despawn()
    {
        this.gameObject.SetActive(false);
    }
    // ham goi khi count down ket thuc
    public void OnCountDownFinished()
    {
        GameManager.ChangeStateOnCountDown();
        GameManager.SetUpJoyStick();
    }
}
