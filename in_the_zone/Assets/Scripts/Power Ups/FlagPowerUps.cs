using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagPowerUps : MonoBehaviour
{
    public int batchId;

    public bool isTriggered;

    public float resetCooldown = 10f;

    public float resetCooldownTimer;

    public ParticleSystem pickUpEffect;
    void Start()
    {
        isTriggered = false;
        resetCooldownTimer = resetCooldown;
    }

    void Update()
    {
        if (isTriggered)
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            resetCooldownTimer -= Time.deltaTime;
            if (resetCooldownTimer <= 0)
            {
                gameObject.SetActive(true);
                resetCooldownTimer = resetCooldown;
                isTriggered = false;
                gameObject.GetComponent<MeshRenderer>().enabled = true;
            }
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.gameObject.GetComponentInChildren<KartController>().drifting)
            {
                StartCoroutine(PickUpEffect());
            }
        }
    }

    IEnumerator PickUpEffect()
    {
        Instantiate(pickUpEffect, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(5f);
    }
}
