using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public Sound[] musicSounds, goodSfxSounds, badSfxSounds;
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
        }
    }

    private void Start()
    {
        PlayMusic("Autumn Leaves");
        musicText.text = "Currently Playing: "+musicSource.clip.name;
        musicSlider.value = 0.15f;
        musicSlider.value = musicSource.volume;
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
    public void PlayRandomVFX(bool isGood)
    {
        Sound[] selectedArray = isGood ? goodSfxSounds : badSfxSounds;

        if (selectedArray.Length == 0)
        {
            Debug.LogWarning("No SFX found in the selected category!");
            return;
        }

        int randomIndex = UnityEngine.Random.Range(0, selectedArray.Length);
        Sound s = selectedArray[randomIndex];

        sfxSource.PlayOneShot(s.clip);
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
