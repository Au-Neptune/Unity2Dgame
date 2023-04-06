using System;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static AudioManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch; 
            s.source.loop = s.loop;
        }
    }

    private void CheckSoundFound(Sound s)
    {
        if (s == null)
            Debug.LogWarning("Sound " + name + " not found!");
        
        return;
    }

    public void Stop()
    {
        foreach (Sound s in sounds)
        {
            s.source.Stop();
        }
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        CheckSoundFound(s);
        s.source.Stop();
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        CheckSoundFound(s);
        s.source.Play();
    }

    public float getPitch(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        CheckSoundFound(s);
        return s.source.pitch;
    }

    public void setPitch(string name, float pitch)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        CheckSoundFound(s);
        s.source.pitch = pitch;
    }
}
