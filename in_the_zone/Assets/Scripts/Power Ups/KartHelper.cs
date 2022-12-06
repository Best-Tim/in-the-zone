using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KartHelper : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject Kart;

    public GameObject GetKart()
    {
        return Kart;
    }
}
