using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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

    [Header("Skyboxes")]
    public Material[] skyboxes;
    public Light directionalLight;
    private int currentSkyboxIndex = 0;
    

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

    private float dayendedTimer = 0f;

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
        if(GameManager.instance.isGameStarted == false)
        {
            return;
        }
        currentTime += Time.deltaTime;
        timerImage.fillAmount = currentTime / dayLength;
        if (currentTime >= dayLength)
        {
            currentTime = dayLength;
            CustomerManager.instance.canSpawn = false;
            if (CustomerManager.instance.currentCustomers == 0 && endDay == false)
            {
                EndDay();
                dayendedTimer = 0f;
                endDay = true;
            }

            dayendedTimer += Time.deltaTime;
            if (dayendedTimer >= 20f)
            {
                foreach (var customer in FindObjectsOfType<CustomerStates>())
                {
                    customer.GetComponent<CustomerStates>()._Leaving = true;
                }
                dayendedTimer = 0f;
            }
            //StartDay();
            
        }

        if (currentTime < dayLength)
        {
            float cycle = currentTime / dayLength;
            UpdateSkyboxAndLighting(cycle);
        }




        if (EconomyManager.instance.isBust)
        {
            endOfTheDayPanel.SetActive(false);
        }

    }

    void UpdateSkyboxAndLighting(float cycle)
    {
        if (cycle >= 0.25f && cycle < 0.5f)
        {
            currentSkyboxIndex = 1;
            directionalLight.color = Color.Lerp(directionalLight.color, new Color(1f, 0.5f, 0.5f), Time.deltaTime);
            directionalLight.intensity = Mathf.Lerp(directionalLight.intensity, 0.7f, Time.deltaTime);
        }
        else if (cycle >= 0.5f && cycle < 0.75f)
        {
            currentSkyboxIndex = 2;
            directionalLight.color = Color.Lerp(directionalLight.color, new Color(0.5f, 0.5f, 1f), Time.deltaTime);
            directionalLight.intensity = Mathf.Lerp(directionalLight.intensity, 0.4f, Time.deltaTime);
        }
        else if (cycle >= 0.75f && cycle < 1f)
        {
            currentSkyboxIndex = 3;
            directionalLight.color = Color.Lerp(directionalLight.color, new Color(0.1f, 0.1f, 0.3f), Time.deltaTime);
            directionalLight.intensity = Mathf.Lerp(directionalLight.intensity, 0.1f, Time.deltaTime);
        }
        else
        {
            currentSkyboxIndex = 0;
            directionalLight.color = Color.Lerp(directionalLight.color, new Color(1f, 1f, 0.8f), Time.deltaTime);
            directionalLight.intensity = Mathf.Lerp(directionalLight.intensity, 1f, Time.deltaTime);
        }

        RenderSettings.skybox = skyboxes[currentSkyboxIndex];
    }

    public void StartDay()
    {
        foreach (var maid in GameObject.FindGameObjectsWithTag("Waiter"))
        {
            maid.GetComponent<WaiterStates>().SetStateIdle();
        }
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
