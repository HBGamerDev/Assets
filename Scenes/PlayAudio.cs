using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayAudio : MonoBehaviour
{
    public string name;

    public Text fileText;

    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        fileText = GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        fileText.text = name;
    }

    void Play()
    {
        audioSource.PlayOneShot(audioSource.clip);
    }
}
