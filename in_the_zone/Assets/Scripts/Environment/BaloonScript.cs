using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaloonScript : MonoBehaviour
{
    public float speed = 2f;
    public float height = 0.05f;
    private float startY;

    private void Start()
    {
        startY = transform.position.y;
    }

    void Update(){
        Vector3 pos = transform.position;
        float newY = startY + height * Mathf.Sin(Time.time * speed);
        transform.position = new Vector3(pos.x, newY, pos.z);
    }
}
