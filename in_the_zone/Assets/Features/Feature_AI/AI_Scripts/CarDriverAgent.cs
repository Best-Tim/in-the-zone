using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;
using Random = UnityEngine.Random;

public class CarDriverAgent : Agent {
    [Header("Training scene or Director spawned?")] [SerializeField]
    private bool localSpawn = false;

    [Header("Reference Objects")] [SerializeField]
    private GameDirector _gameDirector;

    [SerializeField] private KartController _kartController;
    [SerializeField] private GameObject _kart;
    [SerializeField] private GameObject _kartCollider;
    [SerializeField] private AITrackCheckpoints _aiTrackCheckpoints;
    [SerializeField] private Transform _playerHolder;
    [SerializeField] private Transform _spawnPosition;

    [Header("Variables")] [SerializeField] private float cumulativeReward;
    [SerializeField] private float _stuckOnWallTimer;
    [SerializeField] private Vector3 checkpointForward;
    [SerializeField] private Transform currentCheckpoint;
    [SerializeField] private float TimeToReachNextCheckpoint = 30;
    [SerializeField] private float TimeLeft;
    [SerializeField] private int checkpointsReached = 0;
    private Transform checkpointTransform;

    public void DirectorInitialize(GameDirector gameDirector, int spawnNumber) {
        //Reference core usages
        _gameDirector = gameDirector;
        _aiTrackCheckpoints = gameDirector.AITrackCheckpoints;
        _spawnPosition = _gameDirector.TrackSpawnPoints.GetSpawnLocation(spawnNumber);
        //Set up events
        _gameDirector.AITrackCheckpoints.OnWrongCheckpoint += TrackCheckpoints_OnCarWrongCheckpoint;
        _gameDirector.AITrackCheckpoints.OnCorrectCheckpoint += TrackCheckpoints_OnCorrectCheckpoint;
        _gameDirector.AITrackCheckpoints.OnFinishCheckpoint += TrackCheckpoints_OnFinishCheckpoint;
    }

    private void Awake() {
        if (localSpawn) {
            checkpointTransform = _kart.transform;
            _aiTrackCheckpoints.AddPlayer(checkpointTransform);
            _aiTrackCheckpoints.OnWrongCheckpoint += TrackCheckpoints_OnCarWrongCheckpoint;
            _aiTrackCheckpoints.OnCorrectCheckpoint += TrackCheckpoints_OnCorrectCheckpoint;
            _aiTrackCheckpoints.OnFinishCheckpoint += TrackCheckpoints_OnFinishCheckpoint;
        }
    }

    private void TrackCheckpoints_OnCorrectCheckpoint(object sender, AITrackCheckpoints.CarCheckpointEventArgs e) {
        UpdateCheckpoints();
        if (e.carTransform == _kart.transform) {
            checkpointsReached++;
            AddReward(+0.5f);
        }
    }

    private void TrackCheckpoints_OnFinishCheckpoint(object sender, AITrackCheckpoints.CarCheckpointEventArgs e) {
        if (e.carTransform == _kart.transform) {
            AddReward(+0.5f);
            EndEpisode();
        }
    }

    private void TrackCheckpoints_OnCarWrongCheckpoint(object sender, AITrackCheckpoints.CarCheckpointEventArgs e) {
        if (e.carTransform == _kart.transform) {
            //AddReward(-1f);
        }
    }

    public override void OnEpisodeBegin() {
        _kartController.Respawn(_spawnPosition);
        _aiTrackCheckpoints.ResetCheckpoint(checkpointTransform);
        UpdateCheckpoints();
        checkpointsReached = 0;
        _stuckOnWallTimer = 0;
        TimeLeft = TimeToReachNextCheckpoint;
    }


    private void UpdateCheckpoints() {
        checkpointForward = _aiTrackCheckpoints.GetCurrentCheckpoint(checkpointTransform).forward;
        currentCheckpoint = _aiTrackCheckpoints.GetCurrentCheckpoint(checkpointTransform);
        TimeLeft = TimeToReachNextCheckpoint;
    }

    [Header("Observation values")] [SerializeField]
    private float directionDot;

    [SerializeField] private Vector3 diff;

    [SerializeField] private float distance;

    //Keep in mind that each observation here is a vector, which means it is 3 floats, aka 3 inputs for the space size field
    public override void CollectObservations(VectorSensor sensor) {
        directionDot = Vector3.Dot(_kart.transform.forward, checkpointForward);
        sensor.AddObservation(directionDot);

        diff = currentCheckpoint.position - _kart.transform.position;
        sensor.AddObservation(diff / 20);

        distance = Vector3.Distance(_kart.transform.position, currentCheckpoint.position);
        sensor.AddObservation(distance / 20);
    }


    private void Update() {
        TimeLeft -= Time.deltaTime;
        cumulativeReward = GetCumulativeReward();
        if (this.transform.position.y < -4) {
            Debug.Log("Fell down!");
            AddReward(-1f);
            EndEpisode();
        }

        if (TimeLeft < 0f) {
            AddReward(-1f);
            EndEpisode();
        }
    }


    //the actions of the AI, -1 to 1
    public override void OnActionReceived(ActionBuffers actions) {
        float forwardAmount = 0f;
        float turnAmount = 0f;

        switch (actions.DiscreteActions[0]) {
            case 0:
                forwardAmount = 0f;
                break;
            case 1:
                forwardAmount = +1f;
                break;
            case 2:
                forwardAmount = -1f;
                break;
        }

        switch (actions.DiscreteActions[1]) {
            case 0:
                turnAmount = 0f;
                break;
            case 1:
                turnAmount = +1f;
                break;
            case 2:
                turnAmount = -1f;
                break;
        }

        _kartController.SetInputs(forwardAmount, turnAmount);

        //Agent time based penalty
        AddReward(-0.001f);
    }

    #region KB Controls

    //Add keyboard controls to test our reward system
    public override void Heuristic(in ActionBuffers actionsOut) {
        int forwardAction = 0;
        if (Input.GetKey(KeyCode.UpArrow)) {
            forwardAction = 1;
        }

        if (Input.GetKey(KeyCode.DownArrow)) {
            forwardAction = 2;
        }

        int turnAction = 0;
        if (Input.GetKey(KeyCode.RightArrow)) {
            turnAction = 1;
        }

        if (Input.GetKey(KeyCode.LeftArrow)) {
            turnAction = 2;
        }

        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
        discreteActions[0] = forwardAction;
        discreteActions[1] = turnAction;
    }

    #endregion

    //Reward system
    public void WallhitEnter(Collider other) {
        if (other.CompareTag("Wall")) {
            Debug.Log("Hit wall");
            AddReward(-0.5f);
        }

        if (other.CompareTag("Checkpoint")) {
            Debug.Log("Hit checkpoint");
            AddReward(5f);
        }
    }

    public void WallhitStay(Collider other) {
        if (other.CompareTag("Wall")) {
            AddReward(-0.01f);
            _stuckOnWallTimer++;
            if (_stuckOnWallTimer > 10) {
                Debug.Log("Stuck on wall");
                _stuckOnWallTimer = 0;
                AddReward(-1f);
                EndEpisode();
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Wall")) {
            _stuckOnWallTimer = 0;
        }
    }
}