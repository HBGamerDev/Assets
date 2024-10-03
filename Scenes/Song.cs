using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Song", menuName = "Song")]
public class Song : ScriptableObject
{

    public string name;
    public string credit;
    public AudioClip audio;
}
