using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeMenuController : MonoBehaviour
{
    public GameObject foodUpgradeMenuPanel;
    public GameObject cafeUpgradeMenu;
    public GameObject waiterUpgradeMenu;
   // public bool upgradeMenuOpen = false;
    public bool foodUpgradeMenuOpen = false;
    public bool cafeUpgradeMenuOpen = false;
    public bool waiterUpgradeMenuOpen = false;

    public Animator foodAnimator;
    public Animator cafeAnimator;
    public Animator waiterAnimator;

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
            foodUpgradeMenuOpen = true;
            waiterUpgradeMenuOpen = false;
            cafeUpgradeMenuOpen = false;
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
            cafeUpgradeMenuOpen = true;
            waiterUpgradeMenuOpen = false;
            foodUpgradeMenuOpen = false;
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
            waiterUpgradeMenuOpen = true;
            cafeUpgradeMenuOpen = false;
            foodUpgradeMenuOpen = false;
        }
    }
    IEnumerator ClosePanel(GameObject panel, float timer)
    {
        yield return new WaitForSeconds(timer);
        panel.SetActive(false);
    }
}
