using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Character", menuName = "Character")]
public class Character : ScriptableObject
{
    public string name;
    public Sprite render;
    public Transform rig;

    public bool alt;
    public bool selected;
    public Sprite stock;
}
