using UnityEngine.Audio;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public List<Sound> PlayOnStart;

    private List<Sound> currentSounds=new List<Sound>();
    private List<Sound> toDelete = new List<Sound>();
    private List<Sound> toPlay = new List<Sound>();

    #region singleton
    public static AudioManager instance;
    private void Awake()
    {
        if (instance != null){
            Debug.LogWarning("More than one instance of Inventory found !");
            return;
        }
        instance = this;
    }
    #endregion

    private void Start()
    {
        foreach (Sound sound in PlayOnStart)
        {
            Play(sound);
        }
    }

    private void Update()
    {
        foreach(Sound sound in currentSounds)
        {
            if (!sound.source.isPlaying)
            {
                removeSound(sound);
                if (sound.next && sound.nextSound != null)
                    toPlay.Add(sound.nextSound);
            }
        }
        foreach (Sound sound in toDelete)
            currentSounds.Remove(sound);
        foreach (Sound sound in toPlay)
            Play(sound);
        toDelete = new List<Sound>();
        toPlay = new List<Sound>();
    }


    private void updateSound(Sound sound)
    {
        if (sound.source != null)
        {
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }
    }

    public void removeSound(Sound sound)
    {
        if (currentSounds.Contains(sound))
        {
            sound.source.Stop();
            AudioSource source = sound.source;
            sound.source = null;
            Destroy(source);
            toDelete.Add(sound);
        }
    }

    public void addSound(Sound sound)
    {
        if (!currentSounds.Contains(sound))
        {
            currentSounds.Add(sound);
            sound.source = gameObject.AddComponent<AudioSource>();
            updateSound(sound);
        }
    }

    public void Play(Sound sound)
    {
        addSound(sound);
        sound.source.Play();
    }
}
