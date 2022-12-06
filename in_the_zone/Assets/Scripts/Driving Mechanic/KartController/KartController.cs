using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering.PostProcessing;
using Cinemachine;
using Unity.Mathematics;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class KartController : MonoBehaviour {
    public bool ReverseControlsBool;
    public bool speedDebuffBool = false;


    //new input system 
    Vector2 drivingInput;
    int dir;
    VehicleInputActions inputActions;
    InputSystemControls inputSystemControls;


    [Header("Nickname for player")] public String nickname;

    private PostProcessVolume postVolume;
    private PostProcessProfile postProfile;

    [Header("Kart colliders and rigidbody")]
    public Transform kartModel;

    public Transform kartNormal;
    public Rigidbody sphere;

    [Header("Speed")] public float speed;
    public float currentSpeed;
    float rotate, currentRotate;
    int driftDirection;
    float driftPower;
    int driftMode = 0;
    private bool first, second, third, backwards;
    Color c;

    [Header("Bools")] public bool drifting;

    [Header("Parameters")] public float acceleration = 30f;
    [SerializeField] float steering = 80f;
    [SerializeField] float gravity = 10f;
    [SerializeField] float brakingCoef = 15f;
    [SerializeField] float acceleratingCoef = 6f;
    public LayerMask layerMask;

    [Header("Model Parts")] public Transform frontWheels;
    public Transform backWheels;
    public Transform steeringWheel;

    [Header("Particles")] public List<ParticleSystem> primaryParticles = new List<ParticleSystem>();
    public List<ParticleSystem> secondaryParticles = new List<ParticleSystem>();
    public Transform wheelParticles;
    public Transform flashParticles;
    public Color[] turboColors;
    public List<ParticleSystem> gripParticles;
    public List<ParticleSystem> dirtParticles;

    [Header("Checkpoint tracker")] public GameObject lastCheckPoint;
    CheckpointController checkpointController;
    public GameObject checkpointManager;

    [Header("Bumping other players")] public String lastPlayerToBump;
    public int bumps;

    [Header("Paintbar")] public GameObject paintBar;
    public PaintbarController paintBarController;

    public bool isMovementDisabled;

    [SerializeField] private bool isAirborne;
    public float getPaintWhenAirborneRate = 0.01f;
    public float getPaintWhenAirborneInterval = 0.1f;
    private float getPaintWhenAirborneTimer;
    
    //point system
    public PointEventLibrary pel;

    //fmod 
    FMODAudio fMODAudio;
    public CharacterClassManager characterClassManager;

    void Awake() {
        inputActions = new VehicleInputActions();
        fMODAudio = GameObject.Find("AudioManager").GetComponent<FMODAudio>();
        //events for the input system
        inputActions.FindAction("DriftingP"+IsPlayer).performed += Drifting_performed;
        inputActions.FindAction("DriftingP"+IsPlayer).performed += Driving_performed;
        inputSystemControls = FindObjectOfType<InputSystemControls>();
    }

    public void OnEnable() {
        inputActions.Driving.Enable();
    }

    public void OnDisable() {
        inputActions.Driving.Disable();

    }

    private void Driving_performed(InputAction.CallbackContext context) {
        //Disable your debugs on commiting it to dev >:(
        //Debug.Log("I am driving" + context);
    }

    private void Drifting_performed(InputAction.CallbackContext context) {
        if (context.performed || context.started) {
            Debug.Log(context);
        }

        if (context.canceled) { }
        //Disable your debugs on commiting it to dev >:(
        //Debug.Log("Action has been canceled");
    }

    [SerializeField] private bool isAI = false;
    [SerializeField] public int IsPlayer = 1;

    void AccelerateAndBrake() {
        drivingInput = inputActions.FindAction("DrivingP"+IsPlayer).ReadValue<Vector2>();
        // drivingInput = inputActions.Driving.Driving.ReadValue<Vector2>();
        
        //Accelerate        
        if ((speedDebuffBool ? -drivingInput.y : drivingInput.y) > 0)
        {
            speed = acceleration;
            backwards = false;
            //Disable your debugs on commiting it to dev >:(
            //Debug.Log("W key is pressed");
        }

        //Brake
        if ((speedDebuffBool ? -drivingInput.y : drivingInput.y) < 0)
        {
            backwards = true;
            speed = -acceleration * 0.2f;
            //Disable your debugs on commiting it to dev >:(
            //Debug.Log("S key is pressed");
        }
    }

    void Start() {
        if (transform.TryGetComponent(out PaintbarController paintbarController)) {
            paintBarController = paintbarController;
        }
        else {
            Debug.LogWarning("Cannot get component Paintbarcontroller");
        }

        switch (characterClassManager.GetClass())
        {
            case CharacterClassENUM.METAL:
                fMODAudio.PlayMetalMusic();
                break;
            case CharacterClassENUM.SYNTH:
                fMODAudio.PlaySynthMusic();
                break;
            case CharacterClassENUM.DISCO:
                break;
            default:
                break;
        }

        // postVolume = Camera.main.GetComponent<PostProcessVolume>();
        // if (Camera.main.TryGetComponent<PostProcessVolume>(out PostProcessVolume ppv)) {
        //     postProfile = ppv.profile;
        // }

        paintBar = GetComponent<PaintbarManager>().activePaintbar;

        for (int i = 0; i < wheelParticles.GetChild(0).childCount; i++) {
            primaryParticles.Add(wheelParticles.GetChild(0).GetChild(i).GetComponent<ParticleSystem>());
        }

        for (int i = 0; i < wheelParticles.GetChild(1).childCount; i++) {
            primaryParticles.Add(wheelParticles.GetChild(1).GetChild(i).GetComponent<ParticleSystem>());
        }

        foreach (ParticleSystem p in flashParticles.GetComponentsInChildren<ParticleSystem>()) {
            secondaryParticles.Add(p);
        }

        bumps = 0;
        lastPlayerToBump = null;
        checkpointController = GetComponent<CheckpointController>();
        isAirborne = false;
        getPaintWhenAirborneTimer = getPaintWhenAirborneInterval;

        pel = FindObjectOfType<PointEventLibrary>();
    }    

    float GetAxisHorizontal() {
        var steeringInput = inputActions.FindAction("DrivingP"+IsPlayer).ReadValue<Vector2>().x;
        return (ReverseControlsBool ? -1 : 1) * steeringInput;
    }
    void addPaint(float i)
    {
        if (paintBar.TryGetComponent<PaintbarMechanic>(out PaintbarMechanic paintbarMechanic))
        {
            paintbarMechanic.amount += i;
            pel.PublicEventAddPaintBar(nickname);
        }
    }
    void getsPaintWhenDrifting() {
        addPaint(paintBarController.getPaintWhenDriftingRate);
    }
    void getsPaintWhenAirborne()
    {
        addPaint(getPaintWhenAirborneRate);
    }
    void Drift()
    {
        if (inputActions.FindAction("DriftingP"+IsPlayer).inProgress && !drifting && GetAxisHorizontal() != 0)
        {
            drifting = true;
            driftDirection = GetAxisHorizontal() > 0 ? 1 : -1;

            paintBarController.getPaintWhileDrifting = true;

            foreach (ParticleSystem p in primaryParticles) {
                p.startColor = Color.clear;
                p.Play();
            }
            kartModel.parent.DOComplete();
            kartModel.parent.DOPunchPosition(transform.up * .2f, .3f, 5, 1);
        }
        else if (!inputActions.FindAction("DriftingP"+IsPlayer).inProgress && drifting) {
            Boost();
            CancelInvoke("getsPaintWhenDrifting");
        }
    }

    void Update() {
        //Follow Collider
        transform.position = sphere.transform.position - new Vector3(0, 0.4f, 0);

        if (!isMovementDisabled) {
            if (!isAI) {
                AccelerateAndBrake();

            //Steer        
            if (GetAxisHorizontal() != 0)
            {
                dir = GetAxisHorizontal() > 0 ? 1 : -1; 
                float amount = Mathf.Abs((GetAxisHorizontal()));
                Steer(dir, amount);
            }

            //Drift
            if (!isAirborne)
            {
                Drift();
            }
            
            if (drifting)
            {
                float control = (driftDirection == 1) ? ExtensionMethods.Remap(GetAxisHorizontal(), -1, 1, 0, 2) : ExtensionMethods.Remap(GetAxisHorizontal(), -1, 1, 2, 0);
                float powerControl = (driftDirection == 1) ? ExtensionMethods.Remap(GetAxisHorizontal(), -1, 1, .2f, 1) : ExtensionMethods.Remap(GetAxisHorizontal(), -1, 1, 1, .2f);
                Steer(driftDirection, control);
                driftPower += powerControl * 2.5f;

                ColorDrift();
            }

            //Start generating powerbar when drifting by invoking this method
            if (drifting && paintBarController.getPaintWhileDrifting && !paintBarController.isPainting) {
                InvokeRepeating("getsPaintWhenDrifting", 0.4f, 0.4f);
                paintBarController.getPaintWhileDrifting = false;
            }

            if (isAirborne)
            {
                getPaintWhenAirborneTimer -= Time.deltaTime;
                if (getPaintWhenAirborneTimer <= 0)
                {
                    getsPaintWhenAirborne();
                    getPaintWhenAirborneTimer = getPaintWhenAirborneInterval;
                }
            }
            
            //cheat code fix this
            /*
            if (Input.GetButtonUp("Jump") && drifting)
            {
                Boost();
                CancelInvoke("getsPaintWhenDrifting");
            }
            */
            
            if (backwards)
            {
                currentSpeed = Mathf.SmoothStep(currentSpeed, speed, Time.deltaTime * brakingCoef); speed = 0f;
                if (currentSpeed <= 0)
                {
                    currentRotate = Mathf.Lerp(currentRotate, -rotate, Time.deltaTime * 4f); rotate = 0f;
                }
                else
                {
                    currentRotate = Mathf.Lerp(currentRotate, rotate, Time.deltaTime * 4f); rotate = 0f;
                }
            }
            else {
                currentSpeed = Mathf.SmoothStep(currentSpeed, speed, Time.deltaTime * acceleratingCoef);
                speed = 0f;
                currentRotate = Mathf.Lerp(currentRotate, rotate, Time.deltaTime * 4f);
                rotate = 0f;
            }
            }

            if (acceleration <= 0.5f) {
                speed = 0f;
                currentSpeed = 0f;
            }
        }

        //Animations    
        //a) Kart
        if (!drifting) {
            kartModel.localEulerAngles = Vector3.Lerp(kartModel.localEulerAngles,
                new Vector3(0, 90 + (GetAxisHorizontal() * 15), kartModel.localEulerAngles.z), .2f);
        }
        else {
            float control = (driftDirection == 1)
                ? ExtensionMethods.Remap(GetAxisHorizontal(), -1, 1, .5f, 2)
                : ExtensionMethods.Remap(GetAxisHorizontal(), -1, 1, 2, .5f);
            kartModel.parent.localRotation = Quaternion.Euler(0,
                Mathf.LerpAngle(kartModel.parent.localEulerAngles.y, (control * 15) * driftDirection, .2f), 0);
        }

        //b) Wheels
        frontWheels.localEulerAngles = new Vector3(0, (GetAxisHorizontal() * 15), frontWheels.localEulerAngles.z);
        if (!backwards) {
            frontWheels.localEulerAngles += new Vector3(0, 0, sphere.velocity.magnitude / 2);
            backWheels.localEulerAngles += new Vector3(0, 0, sphere.velocity.magnitude / 2);
        }
        else {
            frontWheels.localEulerAngles += new Vector3(0, 0, -sphere.velocity.magnitude / 2);
            backWheels.localEulerAngles += new Vector3(0, 0, -sphere.velocity.magnitude / 2);
        }

        //c) Steering Wheel
        steeringWheel.localEulerAngles = new Vector3(-25, 90, ((GetAxisHorizontal() * 45)));
    }

    private void FixedUpdate()
    {
        //Forward Acceleration
        if (!drifting)
            sphere.AddForce(-kartModel.transform.right * currentSpeed, ForceMode.Acceleration);
        else
            sphere.AddForce(transform.forward * currentSpeed, ForceMode.Acceleration);

        //Gravity
        if (isAirborne)
        {
            sphere.AddForce(Vector3.down * gravity * 2f, ForceMode.Acceleration);
        }
        else
        {
            sphere.AddForce(Vector3.down * gravity, ForceMode.Acceleration);
        }

        //Steering
        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles,
            new Vector3(0, transform.eulerAngles.y + currentRotate, 0), Time.deltaTime * 5f);

        RaycastHit hitOn;
        RaycastHit hitNear;

        if (!Physics.Raycast(transform.position + (transform.up * .1f), Vector3.down, out hitOn, 1f, layerMask))
        {
            isAirborne = true;
            //disable A & D buttons
        }
        else
        {
            if (hitOn.transform.gameObject.layer == 19)
            {
                if (currentSpeed >= acceleration * 0.5f)
                {
                    currentSpeed -= 2;
                }
            }
            isAirborne = false;
        }
        
        Physics.Raycast(transform.position + (transform.up * .1f)   , Vector3.down, out hitNear, 2f, layerMask);
        //Normal Rotation
        kartNormal.up = Vector3.Lerp(kartNormal.up, hitNear.normal, Time.deltaTime * 8.0f);
        kartNormal.Rotate(0, transform.eulerAngles.y, 0);
        
        foreach (ParticleSystem ps in gripParticles)
        {
            if (currentSpeed >= 5 && !isAirborne && hitOn.transform.gameObject.layer != 19)
            {
                ps.Emit(10);
            }
        }
        foreach (ParticleSystem ps in dirtParticles)
        {
            if (hitOn.transform.gameObject.layer == 19)
            {
                ps.Emit(10);
            }
            else
            {
                return;
            }
        }
    }

    public void Boost() {
        drifting = false;

        if (driftMode > 0) {
            DOVirtual.Float(currentSpeed * 1.2f, currentSpeed, .2f * driftMode, Speed);
            kartModel.Find("Tube001").GetComponentInChildren<ParticleSystem>().Play();
            kartModel.Find("Tube002").GetComponentInChildren<ParticleSystem>().Play();
        }

        driftPower = 0;
        driftMode = 0;
        first = false;
        second = false;
        third = false;

        foreach (ParticleSystem p in primaryParticles) {
            p.startColor = Color.clear;
            p.Stop();
        }

        kartModel.parent.DOLocalRotate(Vector3.zero, .5f).SetEase(Ease.OutBack);
    }

    public void Steer(int direction, float amount) {
        rotate = (steering * direction) * amount;
    }

    public void ColorDrift() {
        if (!first)
            c = Color.clear;

        if (driftPower > 50 && driftPower < 100 - 1 && !first) {
            first = true;
            c = turboColors[0];
            driftMode = 1;

            PlayFlashParticle(c);
        }

        if (driftPower > 100 && driftPower < 150 - 1 && !second) {
            second = true;
            c = turboColors[1];
            driftMode = 2;

            PlayFlashParticle(c);
        }

        if (driftPower > 150 && !third) {
            third = true;
            c = turboColors[2];
            driftMode = 3;

            PlayFlashParticle(c);
        }

        foreach (ParticleSystem p in primaryParticles) {
            var pmain = p.main;
            pmain.startColor = c;
        }

        foreach (ParticleSystem p in secondaryParticles) {
            var pmain = p.main;
            pmain.startColor = c;
        }
    }

    void PlayFlashParticle(Color c) {
        if (transform.parent.Find("PlayerCamera")) {
            // GameObject.Find("CM vcam1").GetComponent<CinemachineImpulseSource>().GenerateImpulse();
            transform.parent.Find("PlayerCamera").GetComponentInChildren<CinemachineImpulseSource>().GenerateImpulse();

            foreach (ParticleSystem p in secondaryParticles) {
                var pmain = p.main;
                pmain.startColor = c;
                p.Play();
            }
        }
    }

    private void Speed(float x) {
        currentSpeed = x;
    }

    //Screen blurred during boost - does not work atm
    void ChromaticAmount(float x) {
        postProfile.GetSetting<ChromaticAberration>().intensity.value = x;
    }

    public void SetInputs(float x, float y) {
        int aiForward = (int) x;
        int aiSteering = (int) y;
        if (aiForward == 1) {
            speed = acceleration;
            backwards = false;
        }
        else if (aiForward == -1) {
            backwards = true;
            speed = -acceleration * 0.2f;
        }

        Steer(aiSteering, 2);
    }

    public void Respawn(Transform spawnTransform) {
        //Spawn on a random location
        var x = Random.Range(-3, 3);
        var z = Random.Range(-3, 3);
        Vector3 randomSpawn = spawnTransform.position + new Vector3(x, 0, z);
        transform.rotation = spawnTransform.rotation;
        sphere.transform.forward = spawnTransform.forward;
        currentSpeed = 0;
        //Stop Movement
        Rigidbody rigidbodyLocal = sphere.GetComponent<Rigidbody>();
        rigidbodyLocal.MovePosition(randomSpawn);
        rigidbodyLocal.velocity = Vector3.zero;
        rigidbodyLocal.angularVelocity = Vector3.zero;
    }
}