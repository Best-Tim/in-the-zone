using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour
{
    public TileController tc;
    private float timer = 0;
    private CharacterClassManager characterClassManager;

    //to be used in the future to swap out materials for environment
    [SerializeField] List<Material> materials = new List<Material>();

    private void Start()
    {
        characterClassManager = GameObject.Find("Game_Director").GetComponent<CharacterClassManager>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (tc != null)
        {
            if (timer>=15)
            {
                changeThemeEnv();
                timer = 0;
            }
        }
    }

    void changeThemeEnv()
    {
        switch (characterClassManager.GetClass())
        {
            case CharacterClassENUM.METAL:
                //using colors as placeholders for now, list of materials / meshes should be used in future builds
                ChangeMaterail(materials[0]);
                break;
            case CharacterClassENUM.DISCO:
                ChangeMaterail(materials[1]);
                break;
            case CharacterClassENUM.SYNTH:
                ChangeMaterail(materials[2]);
                break;
            default:
                ChangeMaterail(materials[3]);
                break;
        }
    }
    

    void ChangeMaterail(Material color)
    {
        this.GetComponent<Renderer>().material = color;
    }
}
