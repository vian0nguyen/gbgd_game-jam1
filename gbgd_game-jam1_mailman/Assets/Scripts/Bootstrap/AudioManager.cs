using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource aSource;
    public AudioSource randomizeSource;

    public AudioClip defaultLineBlip;
    public AudioClip defaultLetterBlip;

    //plays sound and can be plugged in (does not change with context)
    public void PlaySoundNoContext(AudioClip clip)
    {
        aSource.PlayOneShot(clip);
    }

    //plays audio from randomized audio source (does not change with context)
    public void PlaySoundNoContextRandomizePitch(AudioClip clip)
    {
        float randomPitch = Random.Range(0f, 2f);
        randomizeSource.pitch = randomPitch;
        randomizeSource.PlayOneShot(clip);
    }

}
