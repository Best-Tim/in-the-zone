using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FlagController : MonoBehaviour
{
    private int currentBatchId;

    private bool batchActive;
    private bool isBatchFinished;
    
    private int count;

    private KartController kartController;

    private PaintbarController paintbarController;

    private PointEventLibrary pel;

    private ScoreController text;
    
    public float batchResetTime = 20f;
    private float batchResetTimeTimer;

    private MiniObjectivesController moc;
    void Start()
    {
        currentBatchId = 0;
        count = 0;
        batchActive = false;
        isBatchFinished = false;
        kartController = GetComponent<KartController>();
        paintbarController = GetComponent<PaintbarController>();
        batchResetTimeTimer = batchResetTime;
        pel = FindObjectOfType<PointEventLibrary>();
        text = GetComponent<ScoreController>();
        moc = GetComponent<MiniObjectivesController>();
    }

    void Update()
    {
        if (batchActive && !isBatchFinished)
        {
            batchResetTimeTimer -= Time.deltaTime;
            if (batchResetTimeTimer <= 0)
            {
                currentBatchId = 0;
                count = 0;
                batchActive = false;
                batchResetTimeTimer = batchResetTime;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Flag"))
        {
            if (kartController.drifting)
            {
                if (currentBatchId == 0 && !batchActive && !other.gameObject.GetComponent<FlagPowerUps>().isTriggered)
                {
                    currentBatchId = other.gameObject.GetComponent<FlagPowerUps>().batchId;
                    other.gameObject.GetComponent<FlagPowerUps>().isTriggered = true;
                    batchActive = true;
                    isBatchFinished = false;
                    count = 1;
                }
                else if (currentBatchId != other.GetComponent<FlagPowerUps>().batchId && batchActive)
                {
                    currentBatchId = other.GetComponent<FlagPowerUps>().batchId;
                    count = 1;
                    batchResetTimeTimer = batchResetTime;
                }
                else if (currentBatchId == other.gameObject.GetComponent<FlagPowerUps>().batchId && !other.gameObject.GetComponent<FlagPowerUps>().isTriggered)
                {
                    other.gameObject.GetComponent<FlagPowerUps>().isTriggered = true;
                    batchActive = true;
                    count++;
                    if (count == other.gameObject.transform.parent.childCount)
                    {
                        StartCoroutine(text.PopUpText("Drift Combo!!!", 2));
                        paintbarController.mechanic.amount += 0.5f;
                        pel.PublicEventAddPointsPowerUps(kartController.nickname);
                        currentBatchId = 0;
                        count = 0;
                        batchActive = false;
                        isBatchFinished = true;
                        batchResetTimeTimer = batchResetTime;
                        moc.AddPointQ1();
                        moc.onTextChange.Invoke();
                    }
                } 
            }
        }
    }
}
