using UnityEngine;
using System;	
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(AudioSource))]
public class SoundController : MonoBehaviour
{
	public enum SoundeType
	{
		Audio,
		Music
	};

	public SoundeType soundType;
	public List<AudioClip> clips;
	public bool playOnAwake = true;
	public bool randomClip = false;
    public bool playOnEnable = false;
	public bool loop = false;
	public float delayOnAwake = 0.0f;
    public Vector2 randDelayOnAwake;
    public bool randomVolume = false;
	[Header("X = min, Y = max")]
	public Vector2 volume = Vector2.one;
	public bool randomPitch = false;
	[Header("X = min, Y = max")]
	public Vector2 pitch = Vector2.one;
	public bool changeClip = false;
	public Vector2 clipChangeTimeRange = new Vector2(0.0f, 5.0f);
    public bool destroyOnEnd;
    public float noiseRadius;

	private int currentClipIndex = 0;
	private bool bStartPlay = false;
	private bool bChangingStoped = false;
	private AudioSource m_audio;
	private float defaultVolume;
	private float invokeStartTime = 0.0f;
	private float invokeTime = 0.0f;

	// Use this for initialization
	void Awake()
	{
		m_audio = GetComponent<AudioSource>();
		
		m_audio.loop = loop;

		if(clips != null && clips.Count > 0)
			m_audio.clip = clips[UnityEngine.Random.Range(0, clips.Count)];

		if(randomVolume)
			m_audio.volume = UnityEngine.Random.Range(volume.x, volume.y);

		if(randomPitch)
			m_audio.pitch = UnityEngine.Random.Range(pitch.x, pitch.y);

		defaultVolume = m_audio.volume;

		if(playOnAwake)
		{
            if(randDelayOnAwake != Vector2.zero)
            {
                m_audio.playOnAwake = false;

				if(randomClip)
					Invoke("PlayRandomSound", UnityEngine.Random.Range(randDelayOnAwake.x, randDelayOnAwake.y));
				else
                	Invoke("PlaySound", UnityEngine.Random.Range(randDelayOnAwake.x, randDelayOnAwake.y));
            }
			else if(delayOnAwake > 0)
			{
				m_audio.playOnAwake = false;

				if(randomClip)
                    Invoke("PlayRandomSound", delayOnAwake);
                else
                    Invoke("PlaySound", delayOnAwake);
			}
			else
			{
				if(randomClip)
					PlayRandomSound();
				else
					PlaySound();
			}
		}
	}

	// Update is called once per frame
	void Update()
	{
		if(!m_audio.enabled)
		{
			if(destroyOnEnd)
			{
				Destroy(gameObject);
			}

			return;
		}

		if(destroyOnEnd)
		{
			if(!m_audio.isPlaying && bStartPlay)
				Destroy(gameObject);
		}
	}

    private void OnEnable()
    {
        if(playOnEnable)
		{
            if(randomClip)
                PlayRandomSound();
            else
                PlaySound();
        }
    }

    public void PlayFromAnimation()
    {
        PlaySound();
    }

	public void PlaySound()
	{
        if (!gameObject.activeSelf || m_audio.clip == null)
            return;

		bStartPlay = true;
		m_audio.volume = defaultVolume;

		if(m_audio.enabled)
			m_audio.Play();

		if(changeClip)
		{
			invokeStartTime = Time.time;
			invokeTime = UnityEngine.Random.Range (clipChangeTimeRange.x, clipChangeTimeRange.y) + m_audio.clip.length;
			Invoke("ChangeClip", invokeTime);
		}
	}

	public void PlaySound(int num)
	{
        if (clips == null || clips.Count <= 0)
            return;

		m_audio.clip = clips[num];
		
		PlaySound();
	}
	
	public void PlaySound(AudioClip clip)
	{
		m_audio.clip = clip;
		
		PlaySound();
	}
	
	public void PlayRandomSound()
	{
        if (clips == null || clips.Count <= 0)
            return;

        int rndNum = UnityEngine.Random.Range(0, clips.Count);
		m_audio.clip = clips[rndNum];
		
		PlaySound();
	}

	public void PlayRandomSound(int start, int end)
	{
        if (clips == null || clips.Count <= 0)
            return;

        int rndNum = UnityEngine.Random.Range(start, end);
		m_audio.clip = clips[rndNum];

		PlaySound();
	}

	public bool IsPlaying()
	{
		return m_audio.isPlaying;
	}

	public void SetLoop(bool bLoop)
	{
		loop = bLoop;
		m_audio.loop = loop;
	}
	
	public void Stop()
	{
		bStartPlay = false;
		
		if(m_audio.enabled)
			m_audio.Stop();
	}

	void ChangeClip()
	{
        if (clips == null || clips.Count <= 0)
            return;

        if (randomClip)
		{
			m_audio.clip = clips[UnityEngine.Random.Range(0, clips.Count)];
		}
		else
		{
            if(currentClipIndex == clips.Count && !loop)
                return;

			m_audio.clip = clips[currentClipIndex % clips.Count];
			currentClipIndex++;
		}
		
		if(randomVolume)
		{
			m_audio.volume = UnityEngine.Random.Range(volume.x, volume.y);
			defaultVolume = m_audio.volume;
		}
		
		if(randomPitch)
			m_audio.pitch = UnityEngine.Random.Range(pitch.x, pitch.y);

		PlaySound();
	}

	public IEnumerator FadeUpVolume(float time)
	{
		PlaySound();

		float endVolume = m_audio.volume;
		float fadeTime = 0.0f;

		m_audio.volume = 0.0f;

		while(fadeTime < time)
		{
			m_audio.volume = endVolume * fadeTime / time;
			fadeTime += Time.deltaTime;
			yield return new WaitForSeconds(Time.deltaTime);
		}

		m_audio.volume = endVolume;
	}

	public IEnumerator FadeDownVolume(float time)
	{
		float startVolume = m_audio.volume;
		float fadeTime = time;

		while(fadeTime > 0.0f)
		{
			m_audio.volume = startVolume * fadeTime / time;
			fadeTime -= Time.deltaTime;
			yield return new WaitForSeconds(Time.deltaTime);
		}
		m_audio.volume = 0.0f;
		Stop();
	}

	public void ResumeChangeClip()
	{
		if(!changeClip)
			return;
		
		bChangingStoped = false;
		invokeStartTime = Time.time;
		Invoke("ChangeClip", invokeTime);
	}

	public void StopChangeClip()
	{
		if(!changeClip)
			return;
		
		CancelInvoke("ChangeClip");
		bChangingStoped = true;
		invokeTime -= (Time.time - invokeStartTime);

		if(invokeTime < 0)
			invokeTime = 0.0f;
	}

	public bool IsChangingStoped()
	{
		return bChangingStoped;
	}
}
