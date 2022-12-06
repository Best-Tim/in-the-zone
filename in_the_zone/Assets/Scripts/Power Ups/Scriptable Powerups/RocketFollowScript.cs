using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class RocketFollowScript : MonoBehaviour
{
    public float speed = 50f;
    public GameObject targetRocket;
    
    public GameObject playerObject;
    public GameObject slowDownPrefab;

    GameObject closestObject;
     
    public float distanceToDestroy = Mathf.Infinity;

    private CrashController crashController;
    private PointEventLibrary pel;
    
    VehicleInputActions inputActions;
    public string playerName;
    private string x;

    private void Start()
    {
        pel = FindObjectOfType<PointEventLibrary>();
        inputActions = new VehicleInputActions();

        int isPlayer = GameObject.Find(playerName).GetComponentInChildren<KartController>().IsPlayer;
        string x = "UsePowerUpP" + isPlayer;
        inputActions.FindAction(x, true).performed += RocketActivation_performed;
        inputActions.Driving.Enable();
        
    }

    private void OnEnable()
    {
    }
    private void OnDisable()
    {
    }

    // public void Coroutine(GameObject rocket, GameObject target)
    // {
    //     playerObject = target;
    //     distanceToDestroy = 0;
    //     StartCoroutine(SendHoming(rocket));
    // }

    public IEnumerator SendHoming(GameObject rocket)
    {
        // while(!Input.GetMouseButtonDown(0))
        // {
        //     yield return null;
        // }
            
        targetRocket = findClosestVisibility();
        playerObject.GetComponent<KartReferenceObtainer>().Kart.GetComponent<KartController>().havePowerup = false;
        playerObject.GetComponent<KartReferenceObtainer>().Canvas.SetActive(false);
         
        while (Vector3.Distance(targetRocket.transform.position, rocket.transform.position) > 0.3f)
        {
            rocket.transform.position += (targetRocket.transform.position - rocket.transform.position).normalized *
                                         (speed * Time.deltaTime);
            rocket.transform.LookAt(targetRocket.transform);
            yield return null;
        }
        pel.PublicEventAddPointsPowerUps(playerObject.name);
        closestObject.GetComponentInChildren<PaintbarController>().RemovePaint(0.2f);
        crashController.isDisabled = true;

        SpawnSlowDownArrows();
        
        Destroy(rocket);
    }
    
    public GameObject findClosestVisibility()
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
                    closestObject = currentObject.GetComponentInChildren<KartController>().gameObject;
                    crashController = currentObject.GetComponentInChildren<CrashController>();
                    distance = currentDistance;
                }
            }
        }
        return closestObject;
    }
    
    private async void SpawnSlowDownArrows()
    {
        GameObject slowArrows = Instantiate(slowDownPrefab, targetRocket.transform);
        slowArrows.transform.position =
            new Vector3(targetRocket.transform.position.x, targetRocket.transform.position.y + 0.8f, targetRocket.transform.position.z);
        await Task.Delay(2000);
        Destroy(slowArrows);
    }

    public void RocketActivation_performed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            distanceToDestroy = 0;
            StartCoroutine(SendHoming(gameObject));

        }
    }
}
