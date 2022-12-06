using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class PowerupScriptScriptable : MonoBehaviour
{
    public PowerupEffect powerupEffect;
    public GameObject pickupEffect;
    private bool isTriggered;
    
    private GameObject uICoin;
    private GameObject powerupCoin;

    public float respawnTime = 15f;
    private float respawnTimeTimer;

    private GameDirector gd;
    private RandomizedPowerUpsSpawner respawner;

    public CoinEnum coinEnum;

    private void Start()
    {
        respawnTimeTimer = respawnTime;
        gd = FindObjectOfType<GameDirector>();
        respawner = gd.GetComponent<RandomizedPowerUpsSpawner>();
        coinEnum = GetComponent<CoinSetter>().CoinEnumGet();

    }

    private void Update()
    {
        if (isTriggered)
        {
            respawnTimeTimer -= Time.deltaTime;
            this.gameObject.GetComponent<MeshRenderer>().enabled = false;
            if (respawnTimeTimer <= 0)
            {
                isTriggered = false;
                respawnTimeTimer = respawnTime;
                Instantiate(respawner.powerUpPrefabs[Random.Range(0, respawner.powerUpPrefabs.Count)], transform.position,
                    Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        // check here for player/enemy
        if (collision.CompareTag("Player") )
        {
            if (!isTriggered && collision.gameObject.GetComponentInParent<KartReferenceObtainer>().Kart.GetComponent<KartController>().havePowerup == false)
            {
                Instantiate(pickupEffect, transform.position, transform.rotation);
                isTriggered = true;
                
                powerupCoin = collision.transform.parent.gameObject.GetComponent<KartReferenceObtainer>().PowerupCoin;

                uICoin = collision.transform.parent.gameObject.GetComponent<KartReferenceObtainer>().Canvas;
                // powerupCoin.GetComponent<Animator>().SetBool("RocketPowerup", false);
                // powerupCoin.GetComponent<Animator>().SetBool("RocketDefault", false);

                if (coinEnum == CoinEnum.rocket)
                {
                    powerupCoin.GetComponent<UICoinScript>().EnableRocketSprites();
                }
                else if(coinEnum == CoinEnum.caltrops)
                {
                    powerupCoin.GetComponent<UICoinScript>().EnableCaltropsSprites();

                }
                else if(coinEnum == CoinEnum.reverse)
                {
                    powerupCoin.GetComponent<UICoinScript>().EnableReverseSprites();
                }
                uICoin.SetActive(true);
                collision.gameObject.GetComponentInParent<KartReferenceObtainer>().Kart.GetComponent<KartController>()
                    .havePowerup = true;
                
                powerupEffect.Apply(collision.transform.parent.gameObject); // For getting the actual Kart, not the first collider
            }
        }

    }
}
