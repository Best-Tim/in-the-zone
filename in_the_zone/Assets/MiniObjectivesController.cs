using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class MiniObjectivesController : MonoBehaviour
{
    public GameObject miniObjText;

    private KartController kartController;

    private GameDirector gd;

    public String quest1Description;
    public int quest1CurrentCount;
    public int quest1FinalCount;
    public float quest1PointsReward;
    private bool isQuest1Completed;

    public String quest2Description;
    public int quest2CurrentCount;
    public int quest2FinalCount;
    public float quest2PointsReward;
    private bool isQuest2Completed;

    public String quest3Description;
    public int quest3CurrentCount;
    public int quest3FinalCount;
    public float quest3PointsReward;
    private bool isQuest3Completed;

    private PointEventLibrary pel;

    public UnityEvent onTextChange;

    private ScoreController text;
    // Start is called before the first frame update
    void Start()
    {
        gd = FindObjectOfType<GameDirector>();
        kartController = GetComponent<KartController>();
        pel = FindObjectOfType<PointEventLibrary>();
        text = GetComponent<ScoreController>();
        onTextChange.AddListener(UpdateText);
        isQuest1Completed = false;
        isQuest2Completed = false;
        isQuest3Completed = false;
        if (gd._playerList.Count >= 2)
        {
            if (kartController.nickname == gd._playerList[1].GetComponent<KartReferenceObtainer>().Kart
                    .GetComponent<KartController>().nickname)
            {
                RectTransform rect = miniObjText.GetComponent<RectTransform>();
                rect.anchoredPosition = new Vector2(-rect.anchoredPosition.x, rect.anchoredPosition.y);
            }
        }

        quest1CurrentCount = 0;
        quest2CurrentCount = 0;
        quest3CurrentCount = 0;
        // miniObjText.GetComponent<TextMeshProUGUI>().text =
        //     $"{quest1Description}:\n{quest1CurrentCount}/{quest1RewardCount}\n \n {quest2Description}:\n{quest2CurrentCount}/{quest2RewardCount}\n \n {quest3Description}:\n{quest3CurrentCount}/{quest3RewardCount}\n \n";
        onTextChange.Invoke();
    }

    void UpdateText()
    {
        miniObjText.GetComponent<TextMeshProUGUI>().text =
            $"{quest1Description}:\n{quest1CurrentCount}/{quest1FinalCount}\n \n {quest2Description}:\n{quest2CurrentCount}/{quest2FinalCount}\n \n {quest3Description}:\n{quest3CurrentCount}/{quest3FinalCount}\n \n";
    }

    public void AddPointQ1()
    {
        quest1CurrentCount++;
        if (quest1CurrentCount >= quest1FinalCount && !isQuest1Completed)
        {
            isQuest1Completed = true;
            quest1CurrentCount = quest1FinalCount;
            pel.PublicEventAddPoints(quest1PointsReward, kartController.nickname);
            StartCoroutine(text.PopUpText("Quest Completed - 250 points!", 5f));
        }
    }
    
    public void AddPointQ2()
    {
        quest2CurrentCount++;
        if (quest2CurrentCount >= quest2FinalCount && !isQuest2Completed)
        {
            isQuest2Completed = true;
            quest2CurrentCount = quest2FinalCount;
            pel.PublicEventAddPoints(quest2PointsReward, kartController.nickname);
            StartCoroutine(text.PopUpText("Quest Completed - 250 points!", 5f));
        }
    }
    
    public void AddPointQ3()
    {
        quest3CurrentCount++;
        if (quest3CurrentCount >= quest3FinalCount && !isQuest3Completed)
        {
            isQuest3Completed = true;
            quest3CurrentCount = quest3FinalCount;
            pel.PublicEventAddPoints(quest3PointsReward, kartController.nickname);
            StartCoroutine(text.PopUpText("Quest Completed - 250 points!", 5f));
        }
    }
}
