using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeMenuController : MonoBehaviour
{
    public GameObject foodUpgradeMenuPanel;
    public GameObject cafeUpgradeMenu;
    public GameObject waiterUpgradeMenu;
    public GameObject checkListMenu;
   // public bool upgradeMenuOpen = false;
    public bool foodUpgradeMenuOpen = false;
    public bool cafeUpgradeMenuOpen = false;
    public bool waiterUpgradeMenuOpen = false;
    public bool checkListMenuOpen = false;

    public Animator foodAnimator;
    public Animator cafeAnimator;
    public Animator waiterAnimator;
    public Animator checkListAnimator;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void FoodUpgradeMenu()
    {
        if (foodUpgradeMenuOpen)
        {
            StartCoroutine(ClosePanel(foodUpgradeMenuPanel, 0.25f));
            foodUpgradeMenuOpen = false;
            foodAnimator.SetTrigger("Close Panel");
        }
        else
        {
            foodUpgradeMenuPanel.SetActive(true);
            cafeUpgradeMenu.SetActive(false);
            waiterUpgradeMenu.SetActive(false);
            checkListMenu.SetActive(false);
            foodUpgradeMenuOpen = true;
            waiterUpgradeMenuOpen = false;
            cafeUpgradeMenuOpen = false;
            checkListMenuOpen = false;
        }
    }
    public void CafeUpgradeMenu()
    {
        if (cafeUpgradeMenuOpen)
        {
            StartCoroutine(ClosePanel(cafeUpgradeMenu, 0.25f));
            cafeUpgradeMenuOpen = false;
            cafeAnimator.SetTrigger("Close Panel");
        }
        else
        {
            cafeUpgradeMenu.SetActive(true);
            foodUpgradeMenuPanel.SetActive(false);
            waiterUpgradeMenu.SetActive(false);
            checkListMenu.SetActive(false);
            cafeUpgradeMenuOpen = true;
            waiterUpgradeMenuOpen = false;
            foodUpgradeMenuOpen = false;
            checkListMenuOpen = false;
        }
    }
    public void WaiterUpgradeMenu()
    {
        if (waiterUpgradeMenuOpen)
        {
            StartCoroutine(ClosePanel(waiterUpgradeMenu, 0.25f));
            waiterUpgradeMenuOpen = false;
            waiterAnimator.SetTrigger("Close Panel");
        }
        else
        {
            waiterUpgradeMenu.SetActive(true);
            foodUpgradeMenuPanel.SetActive(false);
            cafeUpgradeMenu.SetActive(false);
            checkListMenu.SetActive(false);
            waiterUpgradeMenuOpen = true;
            cafeUpgradeMenuOpen = false;
            foodUpgradeMenuOpen = false;
            checkListMenuOpen = false;
        }
    }
    public void CheckListMenu()
    {
        if (checkListMenuOpen)
        {
            StartCoroutine(ClosePanel(checkListMenu, 0.25f));
            checkListMenuOpen = false;
            checkListAnimator.SetTrigger("Close Panel");
        }
        else
        {
            checkListMenu.SetActive(true);
            foodUpgradeMenuPanel.SetActive(false);
            cafeUpgradeMenu.SetActive(false);
            waiterUpgradeMenu.SetActive(false);
            checkListMenuOpen = true;
            cafeUpgradeMenuOpen = false;
            waiterUpgradeMenuOpen = false;
            foodUpgradeMenuOpen = false;
        }
    }
    IEnumerator ClosePanel(GameObject panel, float timer)
    {
        yield return new WaitForSeconds(timer);
        panel.SetActive(false);
    }
}
