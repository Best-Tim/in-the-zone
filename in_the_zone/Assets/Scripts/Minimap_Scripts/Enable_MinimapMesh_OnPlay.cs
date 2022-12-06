using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enable_MinimapMesh_OnPlay : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        this.GetComponent<MeshRenderer>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
