using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DayLoop : MonoBehaviour
{
    public static DayLoop instance;
    private EconomyManager economyManager;

    public float dayLength = 60f;
    private float currentTime = 0f;

    public float dailyRent = 100f;
    public float dailyWages = 50f;
    

    public int day = 1;

    public Image timerImage;
    public Text dayText;


    [SerializeField]
    private int howManySold;
    [SerializeField]
    private float soldAmount;

    public TextMeshProUGUI rentText;
    public TextMeshProUGUI soldAmountText;
    public TextMeshProUGUI wageText;
    public TextMeshProUGUI totalIncome;

    public int totalIncomeInt;
    private bool endDay = false;

    public GameObject endOfTheDayPanel;
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        economyManager = EconomyManager.instance;
        day = 1;
        dayText.text = day.ToString();
        totalIncomeInt = 0;
    }

    void Update()
    {
        currentTime += Time.deltaTime;
        timerImage.fillAmount = currentTime / dayLength;
        if (currentTime >= dayLength)
        {
            currentTime = dayLength;
            CustomerManager.instance.canSpawn = false;
            if (CustomerManager.instance.currentCustomers == 0 && endDay == false)
            {
                EndDay();
                endDay = true;
            }
            //StartDay();
            
        }
        if (EconomyManager.instance.isBust)
        {
            endOfTheDayPanel.SetActive(false);
        }

    }

    public void StartDay()
    {
        CustomerManager.instance.canSpawn = true;
        totalIncomeInt = 0;
        howManySold = 0;
        day++;
        dayText.text = day.ToString();
        currentTime = 0f;
        Time.timeScale = 1f;
        endOfTheDayPanel.SetActive(false);
        endDay = false;
    }

    public void EndDay()
    {
        foreach (var waiter in FindObjectsOfType<WaiterStates>())
        {
            dailyWages += 50f;
        }
        for (int i = 0; i < howManySold; i++)
        {
            soldAmount += 2.5f;
        }
        endOfTheDayPanel.SetActive(true);
        Time.timeScale = 0f;
        int totalCost = (int)(dailyRent + dailyWages + soldAmount);
        economyManager.Pay(totalCost);
        rentText.text = dailyRent.ToString();
        soldAmountText.text = soldAmount.ToString();
        wageText.text = dailyWages.ToString();
        totalIncome.text = totalIncomeInt.ToString();
    }

    public void AddSold()
    {
        howManySold++;
    }

    public void AddIncome(int income)
    {
        totalIncomeInt += income;
    }
}
