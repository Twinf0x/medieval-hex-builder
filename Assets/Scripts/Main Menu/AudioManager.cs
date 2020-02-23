using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public Sound[] sounds;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            Initialize();
        }
        DontDestroyOnLoad(this.gameObject);
    }

    private void Initialize()
    {
        foreach (var sound in sounds)
        {
            sound.source = this.gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }

        Play("Theme");
    }

    public void Play(string name)
    {
        var sound = Array.Find(sounds, s => s.name == name);
        if(sound != null)
            sound.source.Play();
    }

    public void Stop(string name)
    {
        var sound = Array.Find(sounds, s => s.name == name);
        sound?.source.Stop();
    }
}
