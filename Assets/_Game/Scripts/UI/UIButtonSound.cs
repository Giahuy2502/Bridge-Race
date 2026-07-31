using MyNamespace;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonSound : MonoBehaviour
{
    [SerializeField] private FxID clickFx = FxID.SFX_ButtonClick;
    private SoundManager SoundManager => SoundManager.Instance;
    

    public void PlaySound()
    {
        if (SoundManager != null)
        {
            SoundManager.PlayFx(clickFx);
        }
    }
}