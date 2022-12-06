using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPowerUpsInterface
{
    public abstract void FindPlayerInFront();

    public abstract void OnCollision();

    public abstract void Activate(GameObject target, GameObject spawnPoint);

}
