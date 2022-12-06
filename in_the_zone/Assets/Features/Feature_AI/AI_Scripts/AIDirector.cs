using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDirector : MonoBehaviour {
    [Header("Prefabs")]
    [Tooltip("The Object that has the ML_Agent components on it")] [SerializeField]
    private GameObject ai;

    private GameDirector _gameDirector;
    private List<Transform> _aiPlayerList = new List<Transform>();

    public void Initialize_AI(GameDirector gameDirector) {
        _gameDirector = gameDirector;
        int spawnCount = _gameDirector._spawnCounter;
        SpawnAI(spawnCount);
    }


    public List<Transform> GetAIPlayers() {
        return _aiPlayerList;
    }

    private void SpawnAI(int spawnCounter) {
        Transform spawnLocation = _gameDirector.TrackSpawnPoints.GetSpawnLocation(0);
        //Loop through a creation method for the AI holding object
        for (int i = 0; i < _gameDirector.AICount; i++) {

            //Add the AI agent to the Kart
            GameObject aiKart = Instantiate(_gameDirector.KartPrefab, 
                spawnLocation.position, 
                spawnLocation.rotation,
                spawnLocation);
            aiKart.transform.Find("Kart").position = spawnLocation.position;
            aiKart.transform.localScale = new Vector3(1, 1, 1);
            aiKart.name = "AIPlayer_"+i;
            
            //Add the AI to the list
            _aiPlayerList.Add(aiKart.transform);
            
            
            GameObject aiAgent = Instantiate(ai, 
                aiKart.transform.Find("Kart").position, 
                aiKart.transform.Find("Kart").rotation,
                aiKart.transform.Find("Kart").transform);
            aiAgent.gameObject.name = "AI_Agent";
            //Reference the object in the Kart with the rigidbody
            _gameDirector.AITrackCheckpoints.AddPlayer(aiAgent.transform);
            aiAgent.GetComponent<CarDriverAgent>().DirectorInitialize(_gameDirector, 0);
            spawnCounter++;
            
        }
    }
}