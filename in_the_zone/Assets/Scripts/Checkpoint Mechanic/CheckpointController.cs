using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CheckpointController : MonoBehaviour
{
    public GameObject lastCheckpointPosition;
    [SerializeField] private CheckpointManager checkpointManager;
    private GameObject paintBar;
    [SerializeField] private ParticleSystem metalParticles;
    [SerializeField] private ParticleSystem synthParticles;
    [SerializeField] private ParticleSystem discoParticles;
    public CharacterClassManager characterClassManager;
    [SerializeField] private GameDirectorRef _gameDirectorRef;
    
    [SerializeField] private GameObject checkpointReachedTextUI;

    //fmod
    FMODAudio fMODAudio;
    private void Awake()
    {
        checkpointReachedTextUI = GameObject.Find("PopUpText");
        checkpointReachedTextUI.GetComponent<TextMeshProUGUI>().text = "";
        fMODAudio = GameObject.Find("AudioManager").GetComponent<FMODAudio>();
    }

    private void Start()
    {
        checkpointManager = _gameDirectorRef.gameDirector.checkpointManager;
        paintBar = GetComponent<PaintbarManager>().activePaintbar;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Checkpoint checker, checks the id and availability of checkpoints
        //if the checkpoint with id 1 is active -> ontrigger -> checkpoint with id 1 - unavailable, checkpoint with id 2 available
        //if the checkpoint is the last in the list -> make checkpoint 1 active again
        //help pls
        
        CheckpointCollisionChecker(other, paintBar);
    }
    
   public void CheckpointCollisionChecker(Collider target, GameObject paintBar)
    {
        if (target.gameObject.tag == "Checkpoint")
        {
            fMODAudio.PassCheckpoint();

            Checkpoint[] checkpoints = checkpointManager.GetComponent<CheckpointManager>().checkpoints;

            if (target.gameObject.GetComponent<Checkpoint>().isAvailable)
            {               
                target.gameObject.GetComponent<Checkpoint>().isAvailable = false;
                lastCheckpointPosition = target.gameObject;
                
                if (target.GetComponent<Checkpoint>().id == checkpoints.Length)
                {
                    checkpointManager.GetComponent<CheckpointManager>().checkpoints[0]
                        .isAvailable = true;
                }

                foreach (Checkpoint go in checkpoints)
                {
                    if (go.id ==
                        target.GetComponent<Checkpoint>().id + 1 &&
                        target.GetComponent<Checkpoint>().id - 1 < checkpoints.Length)
                    {
                        go.isAvailable = true;
                    }
                }

                switch (characterClassManager.GetClass())
                {
                    case CharacterClassENUM.METAL:
                        metalParticles.Play();
                        break;
                    case CharacterClassENUM.SYNTH:
                        synthParticles.Play();
                        break;
                    case CharacterClassENUM.DISCO:
                        discoParticles.Play();
                        break;
                }
                paintBar.GetComponent<PaintbarMechanic>().amount += 0.5f;
                StartCoroutine(CheckPointTextEnumarator());
            }
        }
    }

   IEnumerator CheckPointTextEnumarator()
   {
       checkpointReachedTextUI.GetComponent<TextMeshProUGUI>().text =
           $"{GetComponent<KartController>().nickname} captured the checkpoint!";
       checkpointReachedTextUI.GetComponent<Animator>().SetTrigger("TriggerText");

       yield return new WaitForSeconds(2f);

       checkpointReachedTextUI.GetComponent<TextMeshProUGUI>().text = "";
   }
}
