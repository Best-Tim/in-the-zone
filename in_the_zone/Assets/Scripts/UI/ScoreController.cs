using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    private PointEventLibrary pel;

    public TextMeshProUGUI scoreText;
    // Start is called before the first frame update
    void Start()
    {
        pel = FindObjectOfType<PointEventLibrary>();
    }

    // Update is called once per frame
    void Update()
    {
        // float currentScore = pel.map[GetComponent<KartController>().nickname];
        // scoreText.text = "Score: " + currentScore.ToString();
    }
    
    public IEnumerator PopUpText(String text, float duration)
    {
        scoreText.text = text;
        scoreText.GetComponent<Animator>().SetTrigger("TriggerText");

        yield return new WaitForSeconds(2f);

        scoreText.GetComponent<TextMeshProUGUI>().text = "";
    }
}
