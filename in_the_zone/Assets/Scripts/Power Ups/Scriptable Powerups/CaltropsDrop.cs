using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CaltropsDrop : MonoBehaviour
{
    // private PlayerInput playerInput;
    public GameObject caltropsPrefab;
    public string playerName;
    VehicleInputActions inputActions;
    public GameObject coinUI;

    private void Start()
    {
        inputActions = new VehicleInputActions();
        coinUI = GameObject.Find(playerName).GetComponent<KartReferenceObtainer>().Canvas;
        int isPlayer = GameObject.Find(playerName).GetComponentInChildren<KartController>().IsPlayer;
        string x = "UsePowerUpP" + isPlayer;
        inputActions.FindAction(x, true).performed += CaltropsActivation_performed;
        inputActions.Driving.Enable();
    }

    public void CaltropsActivation_performed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {

            GameObject droppedCaltrops = Instantiate(caltropsPrefab, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 0.8f ,gameObject.transform.position.z) ,
                caltropsPrefab.transform.rotation);
            
            droppedCaltrops.GetComponent<DroppedCaltropsScript>().playerName = this.playerName;
            coinUI.SetActive(false);
            GameObject.Find(playerName).GetComponentInChildren<KartController>().havePowerup = false;
            Destroy(gameObject); 
        }
        
    }
}
