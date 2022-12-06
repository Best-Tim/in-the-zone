using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class DroppedCaltropsScript : MonoBehaviour
{
    public int duration;
    public GameObject pickupEffect;
    private KartController kartController;
    public string playerName;
    public GameObject slowDownPrefab;
    public Transform spawnPoint;
    public void OnTriggerEnter(Collider collision)
    {
        if ( playerName == collision.transform.parent.gameObject.name)
        {
            return;
        }
        Instantiate(pickupEffect, transform.position, transform.rotation);

        kartController = collision.transform.parent.gameObject.GetComponent<KartReferenceObtainer>().Kart.GetComponent<KartController>();
        spawnPoint = collision.transform.parent.gameObject.GetComponent<KartReferenceObtainer>().Kart.transform;
        Debuff();
        SpawnSlowDownArrows();
        
        Destroy(gameObject);
    }

    private void Speed(float x)
    {
        kartController.currentSpeed = x;
    }
    public void Debuff()
    {
        if (kartController != null)
        {
            
            DOVirtual.Float(kartController.speed, kartController.speed /= 2, duration, Speed);
            
            
        }
    }
    private async void SpawnSlowDownArrows()
    {
        GameObject slowArrows = Instantiate(slowDownPrefab, spawnPoint);
        slowArrows.transform.position =
            new Vector3(spawnPoint.position.x, spawnPoint.position.y + 0.8f, spawnPoint.position.z);
        await Task.Delay(2000);
        Destroy(slowArrows);
    }
}
