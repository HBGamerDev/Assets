using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voice : MonoBehaviour
{
    public string type;

    public AudioClip[] clip;
    public AudioSource audioSource;

    public void Vocalize()
    {
        AudioClip clip = GetRandomClip();
        audioSource.PlayOneShot(clip);
    }

    AudioClip GetRandomClip()
    {
        return clip[UnityEngine.Random.Range(0, clip.Length)];
    }
}
