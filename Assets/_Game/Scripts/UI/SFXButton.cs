using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SFXButton : MonoBehaviour
{
    [SerializeField] private bool isSfxOn;
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private List<Sprite> sprites;
    private DataManager DataManager => DataManager.Instance;
    private SoundManager SoundManager => SoundManager.Instance;

    public void OnInit()
    {
        isSfxOn = DataManager.GetIsMusicOn();
        SetButton(isSfxOn);
    }
    
    public void OnSFXButton()
    {
        isSfxOn = !isSfxOn;
        SoundManager.SetSFXOn(isSfxOn);
        SetButton(isSfxOn);
    }

    private void SetButton(bool isOn)
    {
        if (isOn)
        {
            image.sprite = sprites[1];
            text.text = "Music On";
        }
        else
        {
            image.sprite = sprites[0];
            text.text = "Music Off";
        }
    }
}