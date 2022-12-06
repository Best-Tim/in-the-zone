using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KartReferenceObtainer : MonoBehaviour {
    [Header("In model references")]
    [Tooltip("The Kart inside the prefab so we can reference it with getcomponent on player")]
    [SerializeField]
    private GameObject kart;

    [SerializeField] private GameObject kartCollider;
    [SerializeField] private GameObject objectsCollider;
    [SerializeField] private GameObject paintMechanic;
    [SerializeField] private GameObject playerCamera;
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject powerupCoin;
    

    public GameObject Kart => kart;

    public GameObject KartCollider => kartCollider;

    public GameObject ObjectsCollider => objectsCollider;

    public GameObject PaintMechanic => paintMechanic;
    
    public GameObject PlayerCamera => playerCamera;
    
    public GameObject Canvas => canvas;

    public GameObject PowerupCoin => powerupCoin;
    private void Awake() { }

    private void CheckIfAssigned() {
        if (kart == null) {
            Debug.LogWarning("Variables for KartReferenceObtainer are not assigned!");
        }

        if (kartCollider == null) {
            Debug.LogWarning("Variables for KartReferenceObtainer are not assigned!");
        }

        if (objectsCollider == null) {
            Debug.LogWarning("Variables for KartReferenceObtainer are not assigned!");
        }

        if (paintMechanic == null) {
            Debug.LogWarning("Variables for KartReferenceObtainer are not assigned!");
        }
        if (canvas == null) {
            Debug.LogWarning("Variables for KartReferenceObtainer are not assigned!");
        }
        if (powerupCoin == null) {
            Debug.LogWarning("Variables for KartReferenceObtainer are not assigned!");
        }
    }
}