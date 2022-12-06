using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(menuName = "Powerups/RocketBuff")]
public class RocketBuff : PowerupEffect
{
    public GameObject rocketPrefab;
    public Transform spawnPoint; // to be reviewed
    public Transform targetRocket; // to be reviewed

    public override void Apply(GameObject target)
    {
        spawnPoint = target.GetComponent<KartReferenceObtainer>().Kart.transform;
        GameObject rocket =
            Instantiate(rocketPrefab, spawnPoint);

        rocket.transform.position =
            new Vector3(spawnPoint.position.x, spawnPoint.position.y + 0.8f, spawnPoint.position.z);
        
        rocket.GetComponentInChildren<RocketFollowScript>().playerName = target.name;
        rocket.GetComponent<RocketFollowScript>().playerObject = target;
        // rocket.GetComponent<RocketFollowScript>().RocketActivation_performed();
    }
}

