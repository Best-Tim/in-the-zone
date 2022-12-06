using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    public IPowerUpsInterface activePowerup;
    GameObject spawnPoint;
    public GameObject target;
    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            spawnPoint = this.gameObject;
            // target = GetTarget();
            UsePowerup(target, spawnPoint);
        }
    }

    public void UsePowerup(GameObject target, GameObject spawnPoint)
    {
        if (activePowerup != null)
        {
        activePowerup.Activate(target, spawnPoint);
        // activePowerup = null;
        }
    }
    //
    // public GameObject GetTarget()
    // {
    //     return 
    // }
}
