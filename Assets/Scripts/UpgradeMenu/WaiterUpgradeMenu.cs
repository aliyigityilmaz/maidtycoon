using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaiterUpgradeMenu : MonoBehaviour
{
    public GameObject waiterUpgradeMenu;
    public bool upgradeMenuOpen = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void MenuButton()
    {
        if (upgradeMenuOpen)
        {
            waiterUpgradeMenu.SetActive(false);
            upgradeMenuOpen = false;
        }
        else
        {
            waiterUpgradeMenu.SetActive(true);
            upgradeMenuOpen = true;
        }
    }
}
