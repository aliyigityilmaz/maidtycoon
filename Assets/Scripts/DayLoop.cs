using System.Collections;
using System.Collections.Generic;
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

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        economyManager = EconomyManager.instance;
        day = 1;
        dayText.text = day.ToString();
    }

    void Update()
    {
        currentTime += Time.deltaTime;
        timerImage.fillAmount = currentTime / dayLength;
        if (currentTime >= dayLength)
        {
            EndDay();
            day++;
            dayText.text = day.ToString();
            currentTime = 0f;
            StartDay();
        }
    }

    public void StartDay()
    {

    }

    public void EndDay()
    {
        foreach (var waiter in FindObjectsOfType<WaiterStates>())
        {
            dailyWages += 50f;
        }
        int totalCost = (int)(dailyRent + dailyWages);
        economyManager.Pay(totalCost);
    }
}
