using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMeshDisable : MonoBehaviour
{
    // Start is called before the first frame update
    void Start() {
        this.GetComponent<MeshRenderer>().enabled = false;
    }
}
