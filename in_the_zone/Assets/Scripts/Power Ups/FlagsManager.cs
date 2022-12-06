using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagsManager : MonoBehaviour
{
    public int batchId;
    void Start()
    {
        foreach (FlagPowerUps fpu in GetComponentsInChildren<FlagPowerUps>())
        {
            fpu.batchId = batchId;
        }
    }
}
