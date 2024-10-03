using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlitchedText : MonoBehaviour
{
    public int num;
    public Text digit;
    public BoxCollider2D box;

    void Update()
    {
        digit.text = "" + Random.Range(0, num);
        Warp();
    }

    public void Warp()
    {
        float x;
        float y;
        Vector3 pos;

        x = Random.Range(box.size.x * -50, box.size.x * 50);
        y = Random.Range(box.size.y * -25, box.size.y * 25);
        pos = new Vector3(x, y, -10);

        transform.localPosition = pos;
    }
}
