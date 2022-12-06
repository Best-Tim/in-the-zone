using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher : MonoBehaviour, IPowerUpsInterface
{
    public GameObject rocketPrefab;
    public GameObject spawnPoint; // to be reviewed
    public GameObject target; // to be reviewed
    public float speed = 50f;
    public bool rocketLauncherBool = true;

    private void Start()
    {
        // spawnPoint = GameObject.Find("Kart");
        // target = GameObject.Find("AI_Player");
    }

    private void Update()
    {
        // if (Input.GetMouseButtonDown(0))
        // {
        //     if (target != null)
        //     {
        //         GameObject rocket =
        //         Instantiate(rocketPrefab, spawnPoint.transform.position, rocketPrefab.transform.rotation);
        //         rocket.transform.LookAt(target.transform);
        //         StartCoroutine(SendHoming(rocket));
        //     }
        // }
    }

    // public IEnumerator SendHoming(GameObject rocket)
    // {
    //     while (Vector3.Distance(target.transform.position, rocket.transform.position)>0.3f)
    //     {
    //         rocket.transform.position += (target.transform.position - rocket.transform.position).normalized * (speed * Time.deltaTime);
    //         rocket.transform.LookAt(target.transform);
    //             // rocketLauncherBool = false;
    //             // this.enabled = false;
    //         yield return null;
    //     }
    //     Destroy(rocket);
    // }


    public void Activate(GameObject target, GameObject spawnPoint)
    {
        this.spawnPoint = spawnPoint;
        this.target = target;
        // target = GameObject.Find("AI_Player");
        
        if (this.target != null)
        {
            
            GameObject rocket =
                Instantiate(rocketPrefab, spawnPoint.transform.position, rocketPrefab.transform.rotation);
            rocket.transform.LookAt(target.transform);
            StartCoroutine(SendHoming(rocket));
        }
    }
    
    public IEnumerator SendHoming(GameObject rocket)
    {
        while (Vector3.Distance(target.transform.position, rocket.transform.position)>0.3f)
        {
            rocket.transform.position += (target.transform.position - rocket.transform.position).normalized * (speed * Time.deltaTime);
            rocket.transform.LookAt(target.transform);
            // rocketLauncherBool = false;
            // this.enabled = false;
            yield return null;
        }
        Destroy(rocket);
    }
    
    public void FindPlayerInFront()
    {
        throw new System.NotImplementedException();
    }

    public void OnCollision()
    {
        throw new System.NotImplementedException();
    }
}
