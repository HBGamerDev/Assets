using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Color baseColor;
    public Color offsetColor;

    public SpriteRenderer tile;

    public void Init(bool offset)
    {
        tile.color = offset ? offsetColor : baseColor;
    }
}
