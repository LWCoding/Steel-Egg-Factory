using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{

    public static SoundManager Instance = null;
    public bool IsMuted = false;
    private AudioSource _audioSource;
    private float _volumeBeforeMute;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this);
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayMusic(AudioClip audioClip, float volume)
    {
        if (!IsMuted) { _audioSource.volume = volume; }
        _audioSource.clip = audioClip;
        _audioSource.loop = true;
        _audioSource.Play();
    }

    public void PlayOneShot(AudioClip audioClip, float volume)
    {
        _audioSource.PlayOneShot(audioClip, volume);
    }

    // Toggles the volume between 0 and 1.
    // Sets the IsMuted variable to true (volume on) or false (volume off).
    public void ToggleSound()
    {
        if (!IsMuted) { _volumeBeforeMute = _audioSource.volume; }
        _audioSource.volume = (_audioSource.volume == 0) ? _volumeBeforeMute : 0;
        IsMuted = (_audioSource.volume == 0);
    }
}
