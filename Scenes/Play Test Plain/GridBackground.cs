using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBackground : MonoBehaviour
{
    public int width;
    public int height;

    public Tile tilePrefab;

    // Start is called before the first frame update
    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        for(int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var spawnedTile = Instantiate(tilePrefab, new Vector3(x, y), Quaternion.identity);
                spawnedTile.transform.parent = GetComponent<GridBackground>().transform;
                spawnedTile.transform.localPosition = new Vector3(x, y, 0);

                var offset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                spawnedTile.Init(offset);
            }
        }
    }
}
