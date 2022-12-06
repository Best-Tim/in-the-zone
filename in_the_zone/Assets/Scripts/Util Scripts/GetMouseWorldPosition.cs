using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetMouseWorldPosition : MonoBehaviour
{
    //Get Mouse Position in World with Z = 0f
    public static Vector3 GetMouseWorldPos()
    {
        Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        vec.z = 0f;
        return vec;
    }

    public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
    {
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }
}
