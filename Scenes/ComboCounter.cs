using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboCounter : MonoBehaviour
{
    public Text damage;
    public Text combo;
    public int c;
    public Text totalDamage;
    public int total;

    public void Hit(int d)
    {
        damage.text = "" + d;
        c++;
        combo.text = "" + c;
        total += d;
        totalDamage.text = "" + total;
    }
    
    public void Reset()
    {
        c = 0;
        total = 0;
    }
}
