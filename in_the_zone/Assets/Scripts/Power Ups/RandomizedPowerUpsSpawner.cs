using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizedPowerUpsSpawner : MonoBehaviour
{
    private List<GameObject> spawnPoints;
    public Transform spawnPointParent;

    void Awake()
    {
        spawnPoints = new List<GameObject>();
        foreach (Transform t in spawnPointParent.transform)
        {
            spawnPoints.Add(t.gameObject);
        }
    }

    public List<GameObject> powerUpPrefabs;
    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject go in spawnPoints)
        {
            go.GetComponent<MeshRenderer>().enabled = false;
            Instantiate(powerUpPrefabs[Random.Range(0, powerUpPrefabs.Count)], go.transform.position,
                Quaternion.identity);
        }
    }
}
