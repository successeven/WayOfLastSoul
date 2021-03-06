using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    void Awake()
    {/*
		if (instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
        */
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.loop = s.loop;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.spatialBlend = s.Surround ? 1 : 0;
            s.source.rolloffMode = s.RolloffMode;
            if (s.Surround)
						{
								s.source.minDistance = s.MinDistance;
								s.source.maxDistance = s.MaxDistance;
						}
        }
    }

    public void Stop(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Stop();
    }

    public void StopAll()
    {
        foreach (var item in sounds)
            item.source.Stop();
    }

    public void SetPitch(string sound, float pitch)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " of " + sound + " not found!");
            return;
        }
        s.source.pitch = pitch;
    }


    public bool IsPlaying(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return false;
        }
        return s.source.isPlaying;
    }

    public void Play(string sound, float delay = 0f)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.PlayOneShot(s.clip);
    }

    public void SetVolume(string sound, float value)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.volume = value;
    }

}
