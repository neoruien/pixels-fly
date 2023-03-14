using UnityEngine;
using UnityEngine.Audio;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public Sound[] musicSounds;
    public Sound[] audioSounds;

    public Slider musicSlider;
    public Slider audioSlider;

    private UserData userData;
    private bool switched = false;

    void Start()
    {
        userData = GameObject.Find("Controller").GetComponent<UserData>();
        foreach (Sound s in musicSounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = userData.user.volumes[0];
            s.source.loop = s.loop;
        }

        foreach (Sound s in audioSounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = userData.user.volumes[1];
            s.source.loop = s.loop;
        }

        if (SceneManager.GetActiveScene().name.Equals("Level"))
        {
            musicSounds[0].source.Play(); // Level music
        } else
        {
            musicSounds[1].source.Play(); // Menu music
        }
        
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name.Equals("Level") && !switched)
        {
            switched = true;
            musicSlider = GameObject.Find("Canvas").transform.GetChild(1).GetChild(5).GetComponent<Slider>();
            audioSlider = GameObject.Find("Canvas").transform.GetChild(1).GetChild(7).GetComponent<Slider>();

            musicSlider.value = userData.user.volumes[0];
            audioSlider.value = userData.user.volumes[1];
        }
        else if (SceneManager.GetActiveScene().name.Equals("Menu") && switched)
        {
            switched = false;
        }

        if (SceneManager.GetActiveScene().name.Equals("Level") && GameObject.Find("Canvas").transform.GetChild(1).gameObject.activeInHierarchy)
        {
            foreach (Sound s in musicSounds)
            {
                s.source.volume = musicSlider.value;
            }

            foreach (Sound s in audioSounds)
            {
                s.source.volume = audioSlider.value;
            }

            if (musicSlider.value != userData.user.volumes[0]) userData.user.volumes[0] = musicSlider.value;
            if (audioSlider.value != userData.user.volumes[1]) userData.user.volumes[1] = audioSlider.value;
        }
    }

    /*
    public void LoadLevel()
    {
        //Click();
        musicSounds[1].source.Stop(); // Menu music
        musicSounds[0].source.Play(); // Bkgd music
    }

    public void LoadMenu()
    {
        //Click();
        musicSounds[0].source.Stop(); // Bkgd music
        musicSounds[1].source.Play(); // Menu music
    }

    public void SetMusicVolume()
    {
        
    }

    public void SetAudioVolume()
    {
        
    }
    
    public void Pause()
    {
        //Click();
        musicSounds[0].source.Pause(); // Bkgd music
    }

    public void Resume()
    {
        //Click();
        musicSounds[0].source.UnPause(); // Bkgd music
    }
    */

    public void Click()
    {
        audioSounds[0].source.Play(); // Click audio
    }

    
    public void Replay()
    {
        //Click();
        if (audioSounds[1].source.isPlaying)
        {
            audioSounds[1].source.Stop();
        }
        musicSounds[0].source.Play(); // Bkgd music
    }

    public void Rip()
    {
        musicSounds[0].source.Stop(); // Bkgd music
        audioSounds[1].source.Play(); // Rip audio
        Invoke("Bkgd", 0.5f); // Bkgd music
    }

    public void Bkgd()
    {
        musicSounds[0].source.Play();
    }

    public void Gameover()
    {
        musicSounds[0].source.Stop(); // Bkgd music
        audioSounds[2].source.Play(); // Gameover audio   
    }

    public void Hit()
    {
        musicSounds[0].source.Stop(); // Bkgd music
        audioSounds[3].source.Play(); // Hit audio
    }
}
