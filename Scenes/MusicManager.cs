using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    public AudioSource audio;
    public Song[] songs;

    public Text name;
    public Text credit;

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeSong(int song)
    {
        audio.clip = songs[song].audio;
        audio.Play();
        name.text = songs[song].name;
        credit.text = songs[song].credit;
    }
}
