using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public Sound[] sounds;

    void Awake ()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        // Ambil volume yang tersimpan
        float savedVolume = PlayerPrefs.GetFloat("Volume", 1f); // Default volume = 1
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume * savedVolume; // Mengatur volume berdasarkan nilai yang tersimpan
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    public void Play(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        s.source.Play();
    }
    
    public void Stop(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        s.source.Stop();
    }

    public void SetVolume(float volume)
    {
        foreach (Sound s in sounds)
        {
            s.source.volume = s.volume * volume; // Mengatur volume audio berdasarkan slider
        }
    }
}
