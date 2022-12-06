using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Powerups/CaltropsBuff")]
public class CaltropsBuff : PowerupEffect
{
    public GameObject caltropsPrefap;
    public Vector3 backOfTheCarPosition;
    public GameObject backWheels ;

    public override void Apply(GameObject target)
    {
        Debug.Log(target + "CALTROPS");

        if (target != null)
        {
            Transform playerPosition = target.GetComponent<KartReferenceObtainer>().Kart.transform;
            GameObject caltrops = Instantiate(caltropsPrefap, target.GetComponent<KartReferenceObtainer>().Kart.transform);
            caltrops.transform.position =
                new Vector3(playerPosition.position.x, playerPosition.position.y + 0.8f, playerPosition.position.z);
            caltrops.GetComponentInChildren<CaltropsDrop>().playerName = target.name;

        }

    }
}
