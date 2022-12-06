using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap_Tracking : MonoBehaviour
{
    [SerializeField] private Transform player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = new Vector3(player.position.x, player.position.y + 200, player.position.z);
    }
}
