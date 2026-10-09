using UnityEngine;
using System;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Sources")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;
    public AudioSource[] ambientSources;

    [Header("Audio Clips")]
    public Sound[] bgmSounds;
    public Sound[] sfxSounds;
    public Sound[] ambientSounds;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayBGM("Level");
    }
    public void PlayBGM(string soundName)
    {
        Sound s = Array.Find(bgmSounds, x => x.name == soundName);
        if (s != null)
        {
            if (bgmSource.clip == s.clip && bgmSource.isPlaying)
            {
                return;
            }

            bgmSource.clip = s.clip;
            bgmSource.Play();
        }
    }

    public void PlaySFX(string soundName)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == soundName);
        if (s != null) { sfxSource.PlayOneShot(s.clip); }
    }
    
    public void PlayAmbient(string soundName, float panValue = 0f)
    {
        Sound s = Array.Find(ambientSounds, x => x.name == soundName);
        if (s == null)
        {
            // Debug.LogWarning("Ambient: " + soundName + " ��辺��к�!");
            return;
        }
        
        foreach (AudioSource source in ambientSources)
        {
            if (source.clip == s.clip && source.isPlaying)
            {
                source.panStereo = panValue;
                return;
            }
        }
        
        AudioSource availableSource = Array.Find(ambientSources, x => !x.isPlaying);

        if (availableSource != null)
        {
            availableSource.clip = s.clip;
            availableSource.loop = true;
            availableSource.panStereo = panValue;
            availableSource.Play();
        }
    }
    
    public void StopAmbient(string soundName)
    {
        Sound s = Array.Find(ambientSounds, x => x.name == soundName);
        if (s != null)
        {
            foreach (AudioSource source in ambientSources)
            {
                if (source.clip == s.clip && source.isPlaying)
                {
                    source.Stop();
                }
            }
        }
    }
}