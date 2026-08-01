using System.Collections;
using System.Collections.Generic;
using MyNamespace;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] PlayerData playerData;

    private AudioSource soundSource;
    private List<AudioSource> fxSource = new List<AudioSource>();

    [SerializeField][Range(0f, 1f)] private float volumeBG = 0.75f;
    [SerializeField] private AudioClip[] soundAus;
    [SerializeField][Range(0f, 1f)] private float volumeSFX = 0.75f;
    [SerializeField] private List<AudioClip> fxAus;
    private Coroutine changeSoundCoroutine;

    private bool isLoaded = false;
    private int indexSound;

    public void Awake()
    {
        DontDestroyOnLoad(gameObject);

        soundSource = gameObject.AddComponent<AudioSource>();
        soundSource.loop = true;
    }

    private void Start()
    {
        Invoke(nameof(OnLoad), 1);
    }

    private void OnLoad()
    {
        if (soundAus.Length > 0)
        {
            isLoaded = true;
            PlaySound(SoundID.BG_MainMenu);
        }

        for (int i = 0; i < fxAus.Count; i++)
        {
            AudioSource newFXSource = new GameObject().AddComponent<AudioSource>();
            newFXSource.name = fxAus[i].name;
            newFXSource.clip = fxAus[i];
            newFXSource.volume = volumeSFX;
            newFXSource.loop = false;
            newFXSource.transform.SetParent(transform);
            fxSource.Add(newFXSource);
        }
    }


    public void PlaySound(SoundID ID)
    {
        if (!playerData.ISSoundOn)
        {
            return;
        }
        soundSource.clip = soundAus[(int)ID];
        soundSource.volume = volumeBG;
        soundSource.Play();
    }

    public void PlayFx(FxID ID)
    {
        if (!playerData.ISSFXOn)
        {
            return;
        }
        if (isLoaded)
        {
            if (fxSource[(int)ID] == null)
            {
                fxSource[(int)ID] = new GameObject().AddComponent<AudioSource>();
                fxSource[(int)ID].clip = fxAus[(int)ID];
                fxSource[(int)ID].loop = false;
                fxSource[(int)ID].volume = volumeSFX;
                fxSource[(int)ID].transform.SetParent(transform);
            }
            fxSource[(int)ID].PlayOneShot(fxAus[(int)ID]);

        }
    }

    public void ChangeSound(SoundID ID, float time)
    {
        if (!playerData.ISSoundOn)
        {
            return;
        }
        if (!isLoaded) return;
        if (changeSoundCoroutine != null)
        {
            StopCoroutine(changeSoundCoroutine);
        }
        changeSoundCoroutine = StartCoroutine(ChangeSoundRoutine(ID, time));
    }

    private IEnumerator ChangeSoundRoutine(SoundID ID, float duration)
    {
        float halfDuration = duration / 2f;
        float startVolume = soundSource.volume;
        
        float timer = 0f;
        while (timer < halfDuration)
        {
            timer += Time.deltaTime;
            soundSource.volume = Mathf.Lerp(startVolume, 0f, timer / halfDuration);
            yield return null;
        }
        soundSource.volume = 0f;
        soundSource.clip = soundAus[(int)ID];
        soundSource.Play();
        
        timer = 0f;
        while (timer < halfDuration)
        {
            timer += Time.deltaTime;
            soundSource.volume = Mathf.Lerp(0f, startVolume, timer / halfDuration);
            yield return null;
        }
        soundSource.volume = startVolume;
        changeSoundCoroutine = null;
    }
}