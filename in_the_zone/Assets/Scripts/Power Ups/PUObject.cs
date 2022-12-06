using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PUObject : MonoBehaviour
{
    float rotationSpeed = 1.5f;
    public List<IPowerUpsInterface> powerUpsList;
    public IPowerUpsInterface powerUp;
    
    
    public GameObject holder;
    public GameObject pickupEffect;
    public void Awake()
    {
        transform.DORotate(new Vector3(0, 180, 0), rotationSpeed).SetLoops(-1,LoopType.Incremental);
        // powerUpsList.AddRange(holder.GetComponents<IPowerUpsInterface>());
        powerUp = holder.GetComponent<RocketLauncher>();
        //Debug.LogError("NOTERROR" +powerUp.ToString());
        // Debug.Log("DUMB"+ powerUpsList[0].ToString());
        // powerUpsList.Add();
        // Next Initialize the Powerup list
    }

    public void SetActivePowerUp(Collider other)
    {
        other.gameObject.GetComponent<PowerUpManager>().activePowerup = powerUp; //powerUpsList[0] ; // the list should be randomized
    }

    void OnTriggerEnter (Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            //Debug.LogError("NOTERROR" + other.name);
            // RocketLauncher rocketLauncher = other.GetComponent<RocketLauncher>() ;
            KartController kartController = other.GetComponent<KartHelper>().GetKart().GetComponent<KartController>();
            
            Instantiate(pickupEffect, transform.position, transform.rotation);

            SetActivePowerUp(other);
            
            Destroy(gameObject);

            // Pickup(kartController);
            
        }
    }
}
