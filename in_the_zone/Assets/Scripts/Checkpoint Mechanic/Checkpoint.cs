using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int id;
    public bool isAvailable;

    public List<GameObject> neutralParticles;
    public List<GameObject> metalParticles;
    public List<GameObject> synthParticles;
    public List<GameObject> discoParticles;

    private List<ParticleSystem> particles;
    [SerializeField] private GameObject lightParticles;

    private void Awake()
    {
        List<ParticleSystem> lmfao = lightParticles.GetComponentsInChildren<ParticleSystem>().ToList();
        //Debug.LogWarning(lmfao.Count);
        GameObject gameManager = GameObject.Find("GameManager");
        foreach (GameObject neutral in neutralParticles)
        {
            neutral.SetActive(true);
        }

        foreach (GameObject metal in metalParticles)
        {
            metal.SetActive(false);
        }
        
        foreach (GameObject synth in synthParticles)
        {
            synth.SetActive(false);
        }
        
        foreach (GameObject disco in discoParticles)
        {
            disco.SetActive(false);
        }

        particles = gameObject.GetComponentsInChildren<ParticleSystem>().ToList();
    }

    void Update()
    {
        foreach (ParticleSystem ps in particles)
        {
            if (isAvailable)
            {
                foreach (GameObject neutral in neutralParticles)
                {
                    neutral.SetActive(true);
                }
            }
            else
            {
                foreach (GameObject neutral in neutralParticles)
                {
                    neutral.SetActive(false);
                }
            }
        }
    }
}
