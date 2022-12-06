using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputSystemControls : MonoBehaviour
{
    //add booleans
    [HideInInspector] public bool PowerUpButtonPressed;
    [HideInInspector] public bool PaintbarButtonPressed;
    [HideInInspector] public bool MovementButtonsPressed;
    [HideInInspector] public bool DriftButtonPressed;

    private VehicleInputActions inputActions;

    // Update is called once per frame
    private void Awake()
    {
        inputActions = new VehicleInputActions();
    }
    private void OnDisable()
    {
        inputActions.Driving.Disable();
    }
    private void OnEnable()
    {
        inputActions.Driving.Enable();
    }


}
