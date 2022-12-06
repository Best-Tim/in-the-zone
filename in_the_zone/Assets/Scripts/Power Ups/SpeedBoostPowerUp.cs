using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class SpeedBoostPowerUp : MonoBehaviour
{
    private KartController controller;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            controller = other.transform.parent.gameObject.GetComponentInChildren<KartController>();
            controller.currentSpeed = controller.acceleration * 2f;
        }
    }
    
}
