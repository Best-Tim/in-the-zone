using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackSpawnPoints : MonoBehaviour
{
    [SerializeField]
    private List<Transform> spawnPointList;
    // Start is called before the first frame update
    void Awake()
    {
        Transform spawnPointsTransform = transform;
        spawnPointList = new List<Transform>();
        foreach (Transform spawn in spawnPointsTransform) {
            spawnPointList.Add(spawn);
        }
    }

    public Transform GetSpawnLocation(int id) {
        return spawnPointList[id];
    }
}
