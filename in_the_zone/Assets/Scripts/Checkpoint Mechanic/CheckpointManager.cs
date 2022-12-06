using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public Checkpoint[] checkpoints;
    private Checkpoint[] sortedCheckpoints;
    private int counter;
    private void Awake()
    {
        counter = 1;
        sortedCheckpoints = checkpoints.OrderBy(ob => ob.id).ToArray();
        foreach (Checkpoint checkpoint in sortedCheckpoints)
        {
            checkpoint.id = counter;
            counter++;
        }
    }

    private void Start()
    {
        foreach (Checkpoint checkpoint in sortedCheckpoints)
        {
            sortedCheckpoints[0].isAvailable = true;
        }
    }
}
