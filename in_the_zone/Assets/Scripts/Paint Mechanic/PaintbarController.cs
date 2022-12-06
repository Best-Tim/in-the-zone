using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PaintbarController : MonoBehaviour {
    [Header("Paintbar")] public GameObject paintBar;
    public bool isPainting;
    private bool isPaintingActive;
    public float paintInterval = 0.4f;
    public float paintRate = 0.05f;
    public float getPaintWhenDriftingRate = 0.02f;
    public bool getPaintWhileDrifting;
    VehicleInputActions inputActions;
    public PaintbarMechanic mechanic;

    //public GameObject brush;

    //FMOD
    FMODAudio fMODAudio;
    public CharacterClassManager characterClassManager;

    private void Awake()
    {
        fMODAudio = GameObject.Find("AudioManager").GetComponent<FMODAudio>();
    }

    private void Start() {
        inputActions = new VehicleInputActions();
        if (TryGetComponent(out KartController kartController)) { }
        else {
            Debug.LogWarning("Can't find component");
        }
        
        kartController = this.GetComponent<KartController>();
        int IsPlayer = kartController.IsPlayer;
        string x = "PaintActivationP" + IsPlayer;
        inputActions.FindAction(x, true).performed += PaintActivation_performed;
        paintBar = GetComponent<PaintbarManager>().activePaintbar;
        inputActions.Driving.Enable();
        mechanic = paintBar.GetComponent<PaintbarMechanic>();
    }

    public void PaintActivation_performed(InputAction.CallbackContext context) {
        if (context.performed) {
            if (mechanic.amount >= 0.5f) {
                paintBar.GetComponent<Animator>().SetBool("isUsing", true);
                mechanic.isButtonPressed = true;
                mechanic.paintBarPressButtonText.GetComponent<TextMeshProUGUI>()
                        .text =
                    "";
                isPainting = true;
                isPaintingActive = true;
            }
        }
    }

    public void RemovePaint(float i)
    {
        mechanic.amount -= i;
        
        if (mechanic.amount < 0.5f)
        {
            mechanic.paintBarPressButtonText.GetComponent<TextMeshProUGUI>()
                    .text =
                "";
            paintBar.GetComponent<Animator>().SetBool("isActive", false);
        }
        
        if (mechanic.amount <= 0.02f) {
            CancelInvoke("IsPainting");
            mechanic.isButtonPressed = false;
            mechanic.amount = 0f;
            isPainting = false;
            paintBar.GetComponent<Animator>().SetBool("isUsing", false);
            paintBar.GetComponent<Animator>().SetBool("isActive", false);
            mechanic.paintBarPressButtonText.GetComponent<TextMeshProUGUI>()
                    .text =
                "";
        }
        Debug.LogWarning("LOOOOL");
    }

    void Update() {
        //Paint mechanic with old input system

        /*
        if (inputActions.Driving.PaintActivation.IsPressed())
        {
            if (paintBar.GetComponent<PaintbarMechanic>().amount >= 0.5f)
            {
                isPainting = true;
                isPaintingActive = true;
            }
        }
        */
        if (isPaintingActive) {
            InvokeRepeating("IsPainting", 0f, paintInterval);
            switch (characterClassManager.GetClass())
            {
                case CharacterClassENUM.METAL:
                    fMODAudio.DrainMetalPaint();
                    break;
                case CharacterClassENUM.SYNTH:
                    fMODAudio.DrainSynthPaint();
                    Debug.LogError("This is a synth theme paint!");
                    break;
                case CharacterClassENUM.DISCO:
                    break;
                default:
                    break;
            }
            isPaintingActive = false;
        }

        if (isPainting) {
            if (paintBar.GetComponent<PaintbarMechanic>().amount <= 0.02f) {
                CancelInvoke("IsPainting");
                switch (characterClassManager.GetClass())
                {
                    case CharacterClassENUM.METAL:
                        fMODAudio.StopDrainingMetalPaint();
                    break;
                    case CharacterClassENUM.SYNTH:
                        fMODAudio.StopDrainingSynthPaint();
                        Debug.LogError("This is a synth theme paint!");
                        break;
                    case CharacterClassENUM.DISCO:
                        break;
                    default:
                        break;
                }
                mechanic.isButtonPressed = false;
                mechanic.amount = 0f;
                isPainting = false;
                paintBar.GetComponent<Animator>().SetBool("isUsing", false);
                paintBar.GetComponent<Animator>().SetBool("isActive", false);
                mechanic.paintBarPressButtonText.GetComponent<TextMeshProUGUI>()
                        .text =
                    "";
            }
        }
    }

    void IsPainting() {
        paintBar.GetComponent<PaintbarMechanic>().amount -= paintRate;
        // Instantiate(brush, new Vector3(transform.position.x, transform.position.y - 0.42f, transform.position.z), Quaternion.identity);
    }
}