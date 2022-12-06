using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PowerUp : MonoBehaviour {
    public float duration = 5f;
    public float rotationSpeed = 1.5f;
    public GameObject pickupEffect;

    private void Start() {
        transform.DORotate(new Vector3(0, 180, 0), rotationSpeed).SetLoops(-1, LoopType.Incremental);
    }

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            //Debug.Log(other.name);
            if (other.name == "KartCollider") {
                if (other.TryGetComponent<KartHelper>(out KartHelper kartHelper)) {
                    if (kartHelper.TryGetComponent<KartController>(out KartController kartController)) {
                        KartController reverseControls = kartController;
                        StartCoroutine(Pickup(reverseControls));
                    }
                }
            }
            else {
                Debug.LogWarning("Could not find the karthelper component");
            }
        }
    }

    IEnumerator Pickup(KartController reverseControls) {
        // Spawn a cool effect
        Instantiate(pickupEffect, transform.position, transform.rotation);
        // reverseControls.GetComponent<RocketLauncher>().enabled = true;

        // Apply to the Player

        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<SphereCollider>().enabled = false;
        reverseControls.ReverseControlsBool = true;

        yield return new WaitForSeconds(duration);

        reverseControls.ReverseControlsBool = false;
        reverseControls.GetComponent<RocketLauncher>().enabled = false;
        Destroy(gameObject);
    }
}