using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffects : MonoBehaviour
{

    Object[] sfx;
    AudioSource audio;

    public enum SFX {build_SFX, terraform_SFX};

    void Start()
    {
        audio = GetComponent<AudioSource>();

        sfx = Resources.LoadAll("Sounds/SFX", typeof(AudioClip));
    }

    public void PlaySFX(SFX sound)
    {
        audio.clip = sfx[(int)sound] as AudioClip;
        audio.Play();
    }
}
