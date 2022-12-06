using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCollision : MonoBehaviour
{
    ParticlesController particlesController;
    void Start()
    {
        particlesController = GetComponentInParent<ParticlesController>();   
    }
}