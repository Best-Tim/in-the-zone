using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrashController : MonoBehaviour
{
    [SerializeField] private KartController kartController;
    public GameObject kart;
    public Rigidbody kartRB;
    public GameObject kartRot;

    public bool isDisabled;
    

    private float stunDurationTimer;
    public float stunDuration = 2.5f;

    public ParticleSystem explosionParticles;
    public ParticleSystem fireParticles;

    // Start is called before the first frame update
    void Start()
    {
        stunDurationTimer = stunDuration;
        isDisabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (isDisabled)
        {
            StartCoroutine(ParticlesCoroutine());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // if (other.gameObject.tag == "Explosive")
        // {
        //     if (kartController.currentSpeed >= 0.75f * kartController.acceleration)
        //     {
        //         StartCoroutine(ParticlesCoroutine());
        //         kartController.currentSpeed = 0;
        //         isDisabled = true;
        //         Debug.Log(kartController.currentSpeed);
        //     }
        //     if (kartController.currentSpeed >= 0.3f * kartController.acceleration)
        //     {
        //         kartController.currentSpeed = 0;
        //     }
        // }
    }

    IEnumerator ParticlesCoroutine()
    {
        isDisabled = false;
        Instantiate(explosionParticles, new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z), Quaternion.identity);
        kartController.enabled = false;
        yield return new WaitForSeconds(stunDuration);
        kartController.enabled = true;
        Instantiate(fireParticles, new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z), Quaternion.identity);
    }
}
