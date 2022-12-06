using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCheckpointRay : MonoBehaviour {
    [SerializeField] private List<Transform> checkpointList;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnDrawGizmos() {
        if (checkpointList != null) {
            foreach (Transform checkpoint in checkpointList) {
                Vector3 start = checkpoint.position;
                Vector3 dir = checkpoint.forward;
                Gizmos.DrawRay(start, dir *2);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
