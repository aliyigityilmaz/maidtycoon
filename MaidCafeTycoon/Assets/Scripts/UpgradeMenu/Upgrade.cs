using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour
{
    public bool hasMoney;
    // Start is called before the first frame update
    void Start()
    {
        
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
    public void WaiterSpeed()
    {

    }
    public void RequiredMoney(int moneyAmount)
    {
        if (moneyAmount <= EconomyManager.instance.money)
        {
            hasMoney = true;
            EconomyManager.instance.money -= moneyAmount;
        }
    }
}
