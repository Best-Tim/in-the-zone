using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PaintbarManager : MonoBehaviour
{
    public List<GameObject> paintbars;
    public CharacterClassManager characterClassManager;
    public GameObject activePaintbar;

    private void Awake()
    {
        switch (characterClassManager.GetClass())
        {
            case CharacterClassENUM.METAL:
                foreach (GameObject go in paintbars)
                {
                    if (go.GetComponent<PaintbarMechanic>().theme == 1)
                    {
                        go.SetActive(true);
                        activePaintbar = go;
                    }
                    else
                    {
                        go.SetActive(false);
                    }
                }

                break;
            case CharacterClassENUM.SYNTH:
            {
                foreach (GameObject go in paintbars)
                {
                    if (go.GetComponent<PaintbarMechanic>().theme == 2)
                    {
                        go.SetActive(true);
                        activePaintbar = go;
                    }
                    else
                    {
                        go.SetActive(false);
                    }
                }
                break;
            }
            
            case CharacterClassENUM.DISCO:
            {
                foreach (GameObject go in paintbars)
                {
                    if (go.GetComponent<PaintbarMechanic>().theme == 3)
                    {
                        go.SetActive(true);
                        activePaintbar = go;
                    }
                    else
                    {
                        go.SetActive(false);
                    }
                }
                break;
            }
        }
    }
}
