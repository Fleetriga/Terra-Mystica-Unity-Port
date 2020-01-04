using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{

    Object[] music;
    AudioSource audio;

    public void SetUp(string factionName)
    {
        audio = GetComponent<AudioSource>();

        music = Resources.LoadAll("Sounds/Music/"+factionName, typeof(AudioClip));
    }

    void Update()
    {
        if (!audio.isPlaying && music != null)
            PlayRandomMusic();
    }

    void PlayRandomMusic()
    {
        audio.clip = music[Random.Range(0, music.Length)] as AudioClip;
        audio.Play();
    }

}
