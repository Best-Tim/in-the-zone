using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITempCameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var position = target.position;
        this.transform.position = new Vector3(position.x+15, position.y+10, position.z-15);
        this.transform.LookAt(target);
    }
}
