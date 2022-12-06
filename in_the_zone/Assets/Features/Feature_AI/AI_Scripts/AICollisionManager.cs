using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public class AICollisionManager : MonoBehaviour {
    [SerializeField] private Agent aiAgent;

    private void OnCollisionEnter(Collision collision) {
        if (collision.transform.CompareTag("Wall")) {
            aiAgent.AddReward(-1f);
            aiAgent.EndEpisode();
        }
    }
}
