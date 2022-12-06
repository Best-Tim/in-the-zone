using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Powerups/ReverseBuff")]

public class ReverseBuff : PowerupEffect
{
    public int duration;
    public GameObject playerObject;
    KartController closestObject;
    private CrashController crashController;
    public KartController targetReverse;
    public GameObject slowDownPrefab;
    public Transform spawnPoint;
    VehicleInputActions inputActions;
    public string playerName;


    public override void Apply(GameObject target)
    {
        // if (target.CompareTag("Player 0"))
        // {
            inputActions = new VehicleInputActions();
            playerName = target.name;
            int isPlayer = GameObject.Find(playerName).GetComponentInChildren<KartController>().IsPlayer;
            string x = "UsePowerUpP" + isPlayer;
            inputActions.FindAction(x, true).performed += ReverseActivation_performed;
            inputActions.Driving.Enable();
            
            
            playerObject = target;
            targetReverse = findClosestVisibility();
            // KartController kartController = targetReverse;
            // KartController kartController = target.GetComponent<KartReferenceObtainer>().Kart.GetComponent<KartController>();
            // }
    }

    private async void Reverse(KartController kartController)
    {
        playerObject.GetComponent<KartReferenceObtainer>().Canvas.SetActive(false);
        playerObject.GetComponentInChildren<KartController>().havePowerup = false;
        
        kartController.ReverseControlsBool = true;
        
        GameObject slowArrows = Instantiate(slowDownPrefab, targetReverse.transform);
        slowArrows.transform.position =
            new Vector3(targetReverse.transform.position.x, targetReverse.transform.position.y + 0.8f, targetReverse.transform.position.z);
        
        await Task.Delay(duration);
       
        kartController.ReverseControlsBool = false;
        Destroy(slowArrows);
    }
    
    public KartController findClosestVisibility()
    {
        Transform[] objectArray;
        objectArray = FindObjectOfType<GameDirector>()._playerList.ToArray();  // should work with an array of players
        
        float distance = Mathf.Infinity;
         
        Vector3 position = playerObject.GetComponent<KartReferenceObtainer>().Kart.transform.position;
        
        foreach(Transform currentObject in objectArray)
        {
            if(currentObject.gameObject != playerObject) //   All the children in Player also have the Player tag
            {
                Vector3 distanceCheck = currentObject.transform.position - position;
                float currentDistance = distanceCheck.sqrMagnitude;

                if (currentDistance < distance)
                {
                    closestObject = currentObject.GetComponentInChildren<KartController>();
                    crashController = currentObject.GetComponentInChildren<CrashController>();
                    distance = currentDistance;
                }
            }
        }
        return closestObject;
    }
    public void ReverseActivation_performed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Reverse(targetReverse);
        }
    }
}
