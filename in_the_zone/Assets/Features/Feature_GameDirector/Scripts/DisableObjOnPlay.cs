using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableObjOnPlay : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake() {
        this.enabled = false;
    }
}
