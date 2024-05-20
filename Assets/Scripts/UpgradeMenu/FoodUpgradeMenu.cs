using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodUpgradeMenu : MonoBehaviour
{
    public GameObject foodUpgradeMenu;
    public bool upgradeMenuOpen = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void MenuPanel()
    {
        if (upgradeMenuOpen)
        {
            foodUpgradeMenu.SetActive(false);
            upgradeMenuOpen = false;
        }
        else
        {
            foodUpgradeMenu.SetActive(true);
            upgradeMenuOpen = true;
        }
    }

}
