using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    public static EconomyManager instance;

    public int money = 100;

    public TextMeshProUGUI moneyCountText;


    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        moneyCountText.text = money.ToString();
        if (Input.GetKeyDown(KeyCode.O))
        {
            AddMoney(100);
        }
    }

    public void AddMoney(int amount)
    {
        money += amount;
    }
}
