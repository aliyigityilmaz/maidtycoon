using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour
{
    public bool hasMoney;
    public TextMeshProUGUI priceText;
    public int price;

    // Start is called before the first frame update
    void Start()
    {
        priceText.text = price.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChairUpgrade(GameObject chair)
    {
        if (hasMoney)
        {
            chair.SetActive(true);
            CustomerManager.instance.IncreaseMaxCustomer(2);
            GameManager.instance.UpdateChairList();
            this.gameObject.SetActive(false);
            hasMoney = false;
        }
    }
    public void WaiterSpeed(float multiply)
    {
        if (hasMoney)
        {
            foreach (GameObject agent in GameObject.FindGameObjectsWithTag("Waiter"))
            {
                agent.GetComponent<WaiterStates>().agentSpeed += multiply;
            }
            this.gameObject.SetActive(false);
        }

    }
    public void RequiredMoney(int moneyAmount)
    {
        if (moneyAmount <= EconomyManager.instance.money)
        {
            hasMoney = true;
            EconomyManager.instance.money -= moneyAmount;
        }
    }
    public void FoodPrices(int multiply)
    {

    }
}
