using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketRotate : MonoBehaviour
{
    private GameObject rocket;
    public float rotationSpeed = 120;
    void Start()
    {
        rocket = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        //spin
        Vector3 rotation = rocket.transform.localRotation.eulerAngles;
        rotation.z += rotationSpeed * Time.deltaTime;
        rocket.transform.localRotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);
    }
}
