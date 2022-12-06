using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class TileController : MonoBehaviour
{

    //this should be changed to count the tiles once and return an int, but for now this is fine
    //right now however, it should work as a prototype as the counters should still correctly reflect what paint is most on the area
    public Themes CountTiles()
    {
        //same order as enums . 0 metal, 1 70, 2 synth
        int counter0 = 0;
        int counter1 = 0;
        int counter2 = 0;

        foreach (PaintedBy t in GetComponentsInChildren<PaintedBy>())
        {
            if (t.theme == Themes.Metal)
            {
                counter0++;
            }
            if (t.theme == Themes.Seventy)
            {
                counter1++;
            }
            if (t.theme == Themes.Synth)
            {
                counter2++;   
            }
        }
        
        if (counter0 == counter1 && counter1 == counter2)
        {
            return Themes.Empty;
        }
        if (counter0 > counter1 && counter0 > counter2)
        {
            return Themes.Metal;
        }
        if(counter1 >counter0 && counter1 > counter2)
        {
            return Themes.Seventy;
        }
        if (counter2 > counter0 && counter2 > counter1)
        {
            return Themes.Synth;
        }
        return Themes.Empty;
    }
}
