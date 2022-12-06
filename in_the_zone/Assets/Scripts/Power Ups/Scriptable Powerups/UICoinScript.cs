using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICoinScript : MonoBehaviour
{
    public List<GameObject> rocketSprites;
    public List<GameObject> caltropsSprites;
    public List<GameObject> reverseSprites;

    
    public void EnableRocketSprites()
    {
        Debug.LogWarning("GODPLEAAASE");
        foreach (GameObject s in caltropsSprites)
        {
            s.GetComponent<SpriteRenderer>().enabled = false;
        }
        foreach (GameObject s in reverseSprites)
        {
            s.GetComponent<SpriteRenderer>().enabled = false;

        }
        foreach (GameObject s in rocketSprites)
        {
            s.GetComponent<SpriteRenderer>().enabled = true;

        }
    }
    public void EnableCaltropsSprites()
    {
        foreach (GameObject s in caltropsSprites)
        {
            s.SetActive(true);
        }
        foreach (GameObject s in rocketSprites)
        {
            s.SetActive(false);
        }
        foreach (GameObject s in reverseSprites)
        {
            s.SetActive(false);
        }
    }
    public void EnableReverseSprites()
    {
        foreach (GameObject s in reverseSprites)
        {
            s.SetActive(true);
        }
        foreach (GameObject s in rocketSprites)
        {
            s.SetActive(false);
        }
        foreach (GameObject s in caltropsSprites)
        {
            s.SetActive(false);
        }
    }
}
