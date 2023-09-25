using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//[System.Serializable]
//public class Sound
//{
//    public string name;

//    public AudioClip clip;

//    [Range(0f, 1f)]
//    public float volume;
//    [Range(1f, 3f)]
//    public float pitch;

//    [HideInInspector]
//    public AudioSource source;

//}

//public class AudioManager : MonoBehaviour
//{
//    public Sound[] sounds;

//    void Awake()
//    {
//        foreach (Sound sounds in sounds)
//        {
//            sounds.source = gameObject.GetComponent<AudioSource>();
//            sounds.source.clip = sounds.clip;

//            sounds.source.volume = sounds.volume;
//            sounds.source.pitch = sounds.pitch;
//        }
//    }

//    public void Play(string name)
//    {
//        Sound _sound = Array.Find(sounds, sound => sound.name == name);
//        _sound.source.Play();
//    }
//}
