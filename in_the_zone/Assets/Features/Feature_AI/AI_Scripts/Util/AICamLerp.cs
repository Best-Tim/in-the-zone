using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICamLerp : MonoBehaviour
{
    
    public GameObject objectToFollow;
    public GameObject objectToLookAt;
    
    public float speed = 5.0f;

    // Update is called once per frame
    void Update()
    {
        float interpolation = speed * Time.deltaTime;
        
        Vector3 position = this.transform.position;
        Vector3 target = objectToFollow.transform.position;
        this.transform.position = Vector3.MoveTowards(position, target, interpolation);
        this.transform.LookAt(objectToLookAt.transform.position);
    }
}
