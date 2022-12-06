using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using UnityEngine.Events;

public class PaintbarMechanic : MonoBehaviour
{
    [Header("Unity events")]
    public UnityEvent fillPaintBarEvent;
    
    [Header("Theme")]
    public int theme;
    
    [Header("UI element")]
    private Slider paintBar;
    // [SerializeField] GameObject fillObject;
    public GameObject paintBarAmountText;
    public GameObject paintBarPressButtonText;
    
    [Header("Amount of slider value")]
    public float amount;
    public float currentAmount;
    
    private DOTween _doTween;

    private Animator animator;

    public bool isButtonPressed;

    // Start is called before the first frame update
    void Start()
    {
        paintBar = GetComponent<Slider>();
        paintBar.value = 0;
        
        amount = 0;

        paintBarAmountText.GetComponent<TextMeshProUGUI>().text = "";
        paintBarPressButtonText.GetComponent<TextMeshProUGUI>().text = "";
        
        fillPaintBarEvent.AddListener(FillPaintBarEvent);

        animator = GetComponent<Animator>();

        isButtonPressed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (amount >= 1)
        {
            amount = 1;
        }
        
        if (currentAmount != amount && amount <= 1)
        {
            fillPaintBarEvent.Invoke();
        }
    }

    void FillPaintBarEvent()
    {
        if (paintBar.value <= 1)
        {
            paintBar.DOValue(amount, 1f);
            currentAmount = amount;
            paintBarAmountText.GetComponent<TextMeshProUGUI>().text = $"{Mathf.Round(currentAmount * 100)}";
            if (currentAmount >= 0.5f && !isButtonPressed)
            {
                animator.SetBool("isActive", true);
                paintBar.GetComponent<PaintbarMechanic>().paintBarPressButtonText.GetComponent<TextMeshProUGUI>().text =
                    "Press F to paint!";
            }
        }
    }
}
