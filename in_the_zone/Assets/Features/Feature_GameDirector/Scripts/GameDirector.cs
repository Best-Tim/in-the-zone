using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.Barracuda;
using UnityEngine;
using UnityEngine.Serialization;

public class GameDirector : MonoBehaviour {
    //Need to send this from the character select screen

    #region Game Start Variables

    [Header("Player and AI Amount")] [Tooltip("How many players we spawn in the map")] [SerializeField]
    private int playerCount;

    [Tooltip("How many AI we spawn in the map")] [SerializeField]
    private int aiCount;

    [Tooltip("Do we want to play with and spawn AI?")] [SerializeField]
    private bool playWithAI = false;

    #endregion

    #region Prefabs that the game director needs to spawn in

    [Header("Prefabs we spawn in")] [Tooltip("The Object that will contain the driving scripts")] [SerializeField]
    public List<GameObject> kart;

    [Tooltip("The Object that has the AI Director on it")] [SerializeField]
    private GameObject aiDirectorPrefab;
    
    [Tooltip("The Object that has the AI Director on it")] [SerializeField]
    private GameObject playerCamera;

    #endregion

    #region Referenecs that are co-existing in the scene

    [Header("References from the scene")]
    [Tooltip("The object that has all the spawnpoints as its children")]
    [SerializeField]
    private TrackSpawnPoints spawnPointList;

    [SerializeField] [Tooltip("The parent of the checkpoints that contains the AITrackCheckpoints script")]
    private AITrackCheckpoints aiTrackCheckpoints;
    
    [SerializeField] [Tooltip("The object containing the paintbar manager")]
    private GameObject paintbarManager;
    
    [SerializeField] [Tooltip("The object containing the character class manager")]
    public CharacterClassManager characterClassManager;

    [SerializeField] [Tooltip("The Object in the scene containing the checkpointmanager")]
    public CheckpointManager checkpointManager;

    #endregion

    //Easy access to all the players and AI on the map
    public List<Transform> _playerList = new List<Transform>();
    private AIDirector _aiDirector;
    public int AICount => aiCount;
    public GameObject KartPrefab => kart[0];
    public TrackSpawnPoints TrackSpawnPoints => spawnPointList;
    public AITrackCheckpoints AITrackCheckpoints => aiTrackCheckpoints;

    public LayerMask playerCamMask;
    
    //Counter to decide spot on the race grid
    public int _spawnCounter;

    public Camera MinimapCam;

    private void Awake() {
        _spawnCounter = 0;
        characterSelectionManager csm = FindObjectOfType<characterSelectionManager>();

        //set number of players
        playerCount = Convert.ToInt32(csm.numberofplayers);

        //set enum as empty for check
        CharacterClassENUM ccenum = CharacterClassENUM.EMPTY;

        //Loop through the spawn method x playercount
        for (int i = 0; i < playerCount; i++) {

            if (i == 0)
            {
                ccenum = csm.enumPlayer1; 
            }
            else if (i == 1)
            {
                ccenum = csm.enumPlayer2;
            }
            SpawnPlayer(spawnPointList.GetSpawnLocation(_spawnCounter), i, playerCount, ccenum);
            _playerList[i].GetComponent<KartReferenceObtainer>().Kart.GetComponent<KartController>().checkpointManager =
                checkpointManager.transform.gameObject;
            _spawnCounter++;
        }

        //If play with the AI is selected we spawn in the AI Director
        if (playWithAI) {
            SpawnAIDirector();
        }

        aiTrackCheckpoints.FormLists();

      
    }
    
    //Be handled by the online manager later on
    private void SpawnPlayer(Transform spawnLocation,int currentPlayer, int totalPlayers, CharacterClassENUM playerClass) {
        //Spawn in the player Kart
        GameObject player = new GameObject();
        foreach (GameObject T in kart)
        {
            if (T.GetComponentInChildren<CharacterClassManager>().GetClass() == playerClass)
            {
                player = Instantiate(T, spawnLocation);
            }
        }

        //HARDCODED GOT TO CHANGE!!!!!!!!!!!!!!!!!!!!!!
        
        player.name = $"Player{currentPlayer + 1}";
        player.transform.Find("Kart").position = spawnLocation.position;
        KartController kc = player.transform.Find("Kart").GetComponent<KartController>();
        kc.nickname = player.name;
        kc.IsPlayer = currentPlayer + 1;
        //Add a reference to this director script on the player
        if (player.transform.Find("Kart").TryGetComponent<GameDirectorRef>(out GameDirectorRef gdr)) {
            gdr.gameDirector = this;
        }
        //Add the player to a list for tracking
        aiTrackCheckpoints.AddPlayer(player.transform);
        _playerList.Add(player.transform);

        // GameObject pCam = Instantiate(playerCamera, spawnLocation);
        // pCam.transform.Find("vcam").GetComponent<CinemachineVirtualCamera>().Follow = player.transform.Find("Kart");
        // pCam.transform.Find("vcam").GetComponent<CinemachineVirtualCamera>().LookAt = player.transform.Find("Kart");
        if (player.TryGetComponent(out KartReferenceObtainer kro))
        {
            if (playerCount == 2)
            {
                Camera playerCamera = kro.PlayerCamera.GetComponentInChildren<Camera>();
                if (MinimapCam!=null)
                {
                    MinimapCam.rect = new Rect(0f, 0.75f, 1, 0.3f);
                }
                if (currentPlayer == 0)
                {
                    playerCamera.gameObject.layer = 23;
                    playerCamera.rect = new Rect(0, 0, 0.5f, 1);
                    // canvasPlayer1.renderMode = RenderMode.ScreenSpaceCamera;
                    // canvasPlayer1.worldCamera = playerCamera;
                }

                if (currentPlayer == 1)
                {
                    playerCamera.rect = new Rect(0.5f, 0, 0.5f, 1);
                    playerCamera.cullingMask = playerCamMask;
                    // kro.PlayerCamera.GetComponentInChildren<Camera>().gameObject.layer = 24;
                    foreach (Transform t in kro.PlayerCamera.transform)
                    {
                        t.gameObject.layer = 24;
                    }
                    // canvasPlayer2.renderMode = RenderMode.ScreenSpaceCamera;
                    // canvasPlayer2.worldCamera = playerCamera;
                }
            }
        }
    }

    private void SpawnAIDirector() {
        GameObject aiDirectorTemp = Instantiate(aiDirectorPrefab, transform);
        _aiDirector = aiDirectorTemp.GetComponent<AIDirector>();
        _aiDirector.Initialize_AI(this);
    }
}