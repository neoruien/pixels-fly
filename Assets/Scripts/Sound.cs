using System;
using UnityEngine;
using UnityEngine.Audio;

[Serializable] // allows this custom class to appear in inspector
public class Sound
{
    public string name;

    public AudioClip clip;
    
    [Range(0f, 1f)]
    public float volume;

    public bool loop;

    [HideInInspector] // generated in the Awake method of AudioManager and should not be shown in inspector
    public AudioSource source;
}

