using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public Sound[] musicSounds, vfxSounds;
    public AudioSource musicSource;
    public AudioSource sfxSource;

    private Sound currentMusic;

    public TextMeshProUGUI musicText;

    public Slider musicSlider, sfxSlider;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayMusic("Autumn Leaves");
        musicText.text = "Currently Playing: "+musicSource.clip.name;
    }

    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicSounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        else
        {
            musicSource.clip = s.clip;
            musicSource.Play();
        }
        currentMusic = s;
    }
    public void PlayVFX(string name)
    {
        Sound s = Array.Find(vfxSounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        else
        {
            sfxSource.PlayOneShot(s.clip);
        }
    }

    public void PlayNextMusic()
    {
        musicSource.Stop();
        int index = Array.IndexOf(musicSounds, currentMusic);
        if (index == -1 || index == musicSounds.Length - 1)
        {
            index = 0;
        }
        else
        {
            index++;
        }
        currentMusic = musicSounds[index];
        musicSource.clip = currentMusic.clip;
        musicSource.Play();
        musicText.text = "Currently Playing: " + currentMusic.name;
    }

    public void MusicToggle()
    {
        musicSource.mute = !musicSource.mute;
    }

    public void SFXToggle()
    {
        sfxSource.mute = !sfxSource.mute;
    }

    public void MusicVolume()
    {
        musicSource.volume = musicSlider.value;
    }

    public void SFXVolume()
    {
        sfxSource.volume = sfxSlider.value;
    }
}
