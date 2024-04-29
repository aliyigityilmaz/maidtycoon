using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeMenuController : MonoBehaviour
{
    public GameObject upgradeMenu;
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
        if(upgradeMenuOpen)
        {
            upgradeMenu.SetActive(false);
            upgradeMenuOpen = false;
        }
        else
        {
            upgradeMenu.SetActive(true); 
            upgradeMenuOpen = true;
        }
    }
}
