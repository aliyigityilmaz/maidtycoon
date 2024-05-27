using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour
{
    //[SerializeField] private CheckList checkList;

    public bool hasMoney;
    public TextMeshProUGUI priceText;
    public int price;

    public Image upgradeButton;
    public Sprite greenSprite;
    public Sprite graySprite;


    // Start is called before the first frame update
    void Start()
    {
        priceText.text = price.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (EconomyManager.instance.money >= price)
        {
            upgradeButton.sprite = greenSprite;
        }
        else
        {
            upgradeButton.sprite = graySprite;
        }
    }
    public void ChairUpgrade(GameObject chair)
    {
        if (hasMoney)
        {
            chair.SetActive(true);
            CustomerManager.instance.IncreaseMaxCustomer(2);
            GameManager.instance.UpdateChairList();
            this.gameObject.SetActive(false);
            CheckList.instance.cafeUpgrades++;
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
                //WaiterStates.instance.agentSpeed += multiply;
            }
            this.gameObject.SetActive(false);
            CheckList.instance.waiterUpgrades++;
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
    public void FoodPrices(int number)
    {
        OrderSystem.instance.UpgradeBool(number);
        CheckList.instance.foodUpgrades++;
    }

}
