using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using Color = UnityEngine.Color;

public class ParticlesController : MonoBehaviour
{
    PaintOnWorld paintOnWorld;
    ParticleSystem particleSystem;
    Renderer particleMaterial;
    public bool IsOn = false;
    public List<GameObject> childParticles;

    private void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        paintOnWorld = GetComponentInParent<PaintOnWorld>();
        particleMaterial = GetComponent<Renderer>();

        ChangeParticleColor();
    }
    public void StartParticles()
    {
        StartCoroutine("ParticleEffectsController");
    }
    public void StopParticles()
    {
        particleSystem.Stop();
        StopCoroutine("ParticleEffectsController");
    }

    private void ChangeParticleColor()
    {
        particleMaterial.material.SetColor("_Color", paintOnWorld.color);
        foreach (GameObject particles in childParticles)
        {
            particles.GetComponent<Renderer>().material.SetColor("_Color", paintOnWorld.color);
        }
    }

    private void Update()
    {
        if (IsOn)
        {
            StartParticles();
        }
        if(!IsOn)
        {
            StopParticles();
        }
    }

    public IEnumerator ParticleEffectsController()
    {
        while (true)
        {
            if (!particleSystem.isPlaying)
            {
                particleSystem.Play();
            }
            yield return null;  
        }
    }
}
