using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SoundButton : MonoBehaviour
{
    [SerializeField] private bool isSoundOn;
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private List<Sprite> sprites;
    private DataManager DataManager => DataManager.Instance;
    private SoundManager SoundManager => SoundManager.Instance;

    public void OnInit()
    {
        isSoundOn = DataManager.GetIsSoundOn();
        SetButton(isSoundOn);
    }
    
    public void OnSoundButton()
    {
        isSoundOn = !isSoundOn;
        SoundManager.SetSoundOn(isSoundOn);
        SetButton(isSoundOn);
    }

    private void SetButton(bool isOn)
    {
        if (isOn)
        {
            image.sprite = sprites[1];
            text.text = "Sound On";
        }
        else
        {
            image.sprite = sprites[0];
            text.text = "Sound Off";
        }
    }
}
