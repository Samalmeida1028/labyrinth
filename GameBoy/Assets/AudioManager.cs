using UnityEngine;
using UnityEngine.Audio;
using UnityEditor;
using System;


public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    void Start()
    {
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
    }

    void StopAllAudio()
    {
        foreach (Sound s in sounds) {
            s.source.Stop();
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


}
