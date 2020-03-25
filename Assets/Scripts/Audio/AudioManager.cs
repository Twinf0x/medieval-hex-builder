using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public Sound[] sounds;

    public bool IsMusicOn { get; private set; }
    public bool AreSfxOn { get; private set; } 

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

        IsMusicOn = true;
        AreSfxOn = true;
    }

    public void Play(string name)
    {
        var sound = Array.Find(sounds, s => s.name == name);
        if(sound == null)
        {
            return;
        }

        if(!AreSfxOn && sound.type == SoundType.SFX)
        {
            return;
        }
        
        //As music gets muted through the settings, it doesn't matter if we play it or not
        sound.source.Play();
    }

    public void Pause(string name)
    {
        var sound = Array.Find(sounds, s => s.name == name);
        sound?.source.Pause();
    }

    public void Stop(string name)
    {
        var sound = Array.Find(sounds, s => s.name == name);
        sound?.source.Stop();
    }

    public void StopAll()
    {
        foreach(Sound sound in sounds)
        {
            sound.source.Stop();
        }
    }

    public void ToggleMusic()
    {
        IsMusicOn = !IsMusicOn;

        if(!IsMusicOn)
        {
            PauseAllMusic();
        }
        else
        {
            ResumeAllMusic();
        }
    }

    public void ToggleSfx()
    {
        AreSfxOn = !AreSfxOn;

        if(!AreSfxOn)
        {
            StopAllSfx();
        }
    }

    private void PauseAllMusic()
    {
        foreach(Sound sound in sounds)
        {
            if(sound.type != SoundType.MUSIC)
            {
                continue;
            }

            sound.source.mute = true;
        }
    }

    private void ResumeAllMusic()
    {
        foreach(Sound sound in sounds)
        {
            if(sound.type != SoundType.MUSIC)
            {
                continue;
            }

            sound.source.mute = false;
        }
    }

    private void StopAllSfx()
    {
        foreach(Sound sound in sounds)
        {
            if(sound.type != SoundType.SFX)
            {
                continue;
            }

            sound.source.Stop();
        }
    }
}
