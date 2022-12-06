using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class StartCountdownController : MonoBehaviour
{
    public int coundownTimer;
    public TextMeshProUGUI countdownText;
    [SerializeField] private List<KartController> karts;

    private void Awake()
    {
        foreach (Transform t in GetComponent<GameDirector>()._playerList)
        {
           karts.Add(t.gameObject.GetComponentInChildren<KartController>());
        }
    }

    void Start()
    {
        foreach (KartController kart in karts)
        {
            kart.isMovementDisabled = true;
        }
        StartCoroutine(CountdownToStart());
    }

    IEnumerator CountdownToStart()
    {
        while (coundownTimer > 0)
        {
            countdownText.text = coundownTimer.ToString();

            yield return new WaitForSeconds(1f);

            coundownTimer--;
        }

        countdownText.text = "GO!";

        BeginGame();

        yield return new WaitForSeconds(1f);

        countdownText.text = "";
    }

    void BeginGame()
    {
        foreach (KartController kart in karts)
        {
            kart.isMovementDisabled = false;
        }
    }
}
