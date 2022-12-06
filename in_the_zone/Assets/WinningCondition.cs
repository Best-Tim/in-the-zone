using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinningCondition : MonoBehaviour
{
    public GameObject panel;
    [SerializeField] private GameObject winText;

    private PointEventLibrary pel;
    private GameDirector gd;
    [SerializeField] private PointUIController pointController;
    
    
    // Start is called before the first frame update
    void Start()
    {
        pel = GetComponent<PointEventLibrary>();
        panel.SetActive(false);
        gd = GetComponent<GameDirector>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        PointUIController.onWin += OnWin;
    }

    private void OnDisable()
    {
        PointUIController.onWin -= OnWin;
    }

    public void OnWin(string name)
    {
        foreach (Transform t in gd._playerList)
        {
            Rigidbody rb;
            rb = t.gameObject.GetComponent<KartReferenceObtainer>().KartCollider.GetComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
        //Debug.LogWarning(name);
        panel.SetActive(true);
        winText.GetComponent<TextMeshProUGUI>().text = name + "wins!";
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene("selection");
    }

    public void Quit()
    {
        Application.Quit();
    }
    
}
