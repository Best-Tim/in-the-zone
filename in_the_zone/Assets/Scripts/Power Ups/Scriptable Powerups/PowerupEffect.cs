using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerupEffect : ScriptableObject
{
    private GameObject collisionObject;
    public string playerName;
    public abstract void Apply(GameObject target);
}
