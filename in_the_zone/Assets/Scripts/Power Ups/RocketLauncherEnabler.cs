using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class RocketLauncherEnabler : MonoBehaviour
{
    public float multiplier = 1.4f;
    public float duration = 5f;
    public float rotationSpeed = 1.5f;
    public GameObject pickupEffect;

    private void Start()
    {
        transform.DORotate(new Vector3(0, 180, 0), rotationSpeed).SetLoops(-1,LoopType.Incremental);
    }

    void OnTriggerEnter (Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            //Debug.Log(other.name);
            // RocketLauncher rocketLauncher = other.GetComponent<RocketLauncher>() ;
            KartController kartController = other.GetComponent<KartHelper>().GetKart().GetComponent<KartController>() ;

            Pickup(kartController);
            
        }
    }

    void Pickup (KartController kartController)
    {
        
        // Spawn a cool effect
        Instantiate(pickupEffect, transform.position, transform.rotation);
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<SphereCollider>().enabled = false;    

        // Apply to the Player

        kartController.GetComponent<RocketLauncher>().enabled = true;


        Destroy(gameObject);
    }
}
