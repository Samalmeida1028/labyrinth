using UnityEngine;
using UnityEngine.Audio;
using UnityEditor;
using System;


public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance;



    void Start()
    {
        Screen.SetResolution(64,64,false);
        Play("TitleMusic");
    }

    void Awake() {
        foreach (Sound s in sounds) {
            s.source=gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            
            s.source.loop = s.loop;
            s.source.volume = s.volume;

            s.source.pitch = s.pitch;
            s.source.playOnAwake = false;
        }
        if (instance == null) {
            instance = this;
        
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StopAllAudio()
    {
        foreach (Sound s in sounds) {
            s.source.Stop();
            return;
        }
    }

    public void Play(string name) {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null) {
            Debug.Log("Couldn't find audio!");
            return;
        }
        s.source.Play();
    }

    public void PlayOneShot(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("Couldn't find audio!");
            return;
        }

        s.source.PlayOneShot(s.clip);
    }



}
