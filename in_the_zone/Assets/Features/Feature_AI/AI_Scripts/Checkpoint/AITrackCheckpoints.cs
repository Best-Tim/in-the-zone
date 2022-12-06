using System;
using System.Collections.Generic;
using UnityEngine;

public class AITrackCheckpoints : MonoBehaviour {
    public event EventHandler<CarCheckpointEventArgs> OnCorrectCheckpoint;
    public event EventHandler<CarCheckpointEventArgs> OnWrongCheckpoint;
    public event EventHandler<CarCheckpointEventArgs> OnFinishCheckpoint;
    [SerializeField] private bool waitForSpawn = false;

    public class CarCheckpointEventArgs : EventArgs {
        public Transform carTransform;
    }

    [SerializeField] private List<Transform> playerTransformList;
    private List<AICheckpointSingle> _checkpointSingleList;
    private List<int> nextCheckpointSingleIndexList;

    private void Start() {
        // waitForSpawn = true;
        FormLists();
    }

    public void FormLists() {
        Transform checkpointsTransform = transform;

        _checkpointSingleList = new List<AICheckpointSingle>();
        if (checkpointsTransform != null) {
            foreach (Transform checkpointSingleTransform in checkpointsTransform) {
                AICheckpointSingle checkpointSingle = checkpointSingleTransform.GetComponent<AICheckpointSingle>();
                checkpointSingle.SetTrackCheckpoints(this);
                _checkpointSingleList.Add(checkpointSingle);
            }
        }

        if (playerTransformList is {Count: > 0}) {
            nextCheckpointSingleIndexList = new List<int>();
            foreach (Transform player in playerTransformList) {
                nextCheckpointSingleIndexList.Add(0);
            }
        }

        waitForSpawn = true;
    }

    public Transform GetCurrentCheckpoint(Transform playerTransform) {
        int nextCheckpointSingleIndex = nextCheckpointSingleIndexList[
            playerTransformList
                .IndexOf(playerTransform)];
        return _checkpointSingleList[nextCheckpointSingleIndex].transform;
    }

    public void PlayerThroughCheckpoint(AICheckpointSingle aiCheckpointSingle, Transform playerTransform) {
        if (waitForSpawn) {
            Transform playerFromAi = playerTransform;
            int playerIndex = playerTransformList.IndexOf(playerFromAi);
            int nextCheckpointSingleIndex = nextCheckpointSingleIndexList[playerIndex];

            if (_checkpointSingleList.IndexOf(aiCheckpointSingle) == nextCheckpointSingleIndex) {
                if (nextCheckpointSingleIndex == _checkpointSingleList.Count - 1) {
                    OnFinishCheckpoint?.Invoke(this, new CarCheckpointEventArgs {carTransform = playerFromAi});
                }
                else {
                    OnCorrectCheckpoint?.Invoke(this, new CarCheckpointEventArgs {carTransform = playerFromAi});
                }

                //Correct checkpoint, % is a modulo that checks if we pass the count, is Y, reset to 0
                nextCheckpointSingleIndexList[playerTransformList.IndexOf(playerFromAi)] =
                    (nextCheckpointSingleIndex + 1) % _checkpointSingleList.Count;
            }
            else {
                // Wrong Checkpoint
                OnWrongCheckpoint?.Invoke(this, new CarCheckpointEventArgs {carTransform = playerFromAi});
            }

            try { }
            catch (Exception e) {
                Debug.LogWarning("Wrong Transform send to checkpoint, use:" + playerTransformList[0].name);
                throw;
            }
        }
    }

    public void AddPlayer(Transform player) {
        this.playerTransformList.Add(player);
        FormLists();
    }

    public void ResetCheckpoint(Transform t) {
        var indexOf = playerTransformList.IndexOf(t);
        nextCheckpointSingleIndexList[indexOf] = 0;
    }
}