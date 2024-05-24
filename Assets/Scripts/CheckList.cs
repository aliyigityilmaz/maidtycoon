using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckList : MonoBehaviour
{
    public static CheckList instance;

    public int cafeUpgrades = 0;
    public int cafeUpgradesThisLevel;
    public int foodUpgrades = 0;
    public int foodUpgradesThisLevel;
    public int waiterUpgrades = 0;
    public int waiterUpgradesThisLevel;
    public int soldFoodCount = 0;
    public int requiredFoodCount;

    [Header("Check List")]
    public bool allCafeUpgrades = false;
    public bool allWaiterUpgrades = false;
    public bool allFoodUpgrades = false;
    public TextMeshProUGUI foodCountText;

    public bool cafeUpAlreadyMade = false;
    public bool waiterUpAlreadyMade = false;
    public bool foodUpAlreadyMade = false;
    public bool soldEnoughFood = false;

    public GameObject nextLevelButton;

    public GameObject cafeTick;
    public GameObject waiterTick;
    public GameObject foodTick;
    public GameObject soldFoodTick;
    public GameObject pointingArrow;

    public int cafeCount;
    public int waiterCount;
    public int foodCount;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (cafeUpgrades == cafeUpgradesThisLevel)
        {
            allCafeUpgrades = true;
        }
        if (foodUpgrades == foodUpgradesThisLevel)
        {
            allFoodUpgrades = true;
        }
        if (waiterUpgrades == waiterUpgradesThisLevel)
        {
            allWaiterUpgrades = true;
        }

        if (allCafeUpgrades && !cafeUpAlreadyMade)
        {
            AllCafeUpgrades();
        }
        if (allWaiterUpgrades && !waiterUpAlreadyMade)
        {
            AllWaiterUpgrades();
        }
        if (allFoodUpgrades && !foodUpAlreadyMade)
        {
            AllFoodUpgrades();
        }
        if (soldEnoughFood && foodUpAlreadyMade && waiterUpAlreadyMade && cafeUpAlreadyMade)
        {
            CanGoToNextLevel();
        }
        if (soldFoodCount >= requiredFoodCount)
        {
            SoldFoodCount();
        }
    }
    public void AllCafeUpgrades()
    {
        cafeTick.SetActive(true);
        cafeUpAlreadyMade = true;
    }
    public void AllWaiterUpgrades()
    {
        waiterTick.SetActive(true);
        waiterUpAlreadyMade = true;
    }
    public void AllFoodUpgrades()
    {
        foodTick.SetActive(true);
        foodUpAlreadyMade = true;
    }
    public void SoldFoodCount()
    {
        soldEnoughFood = true;
        soldFoodTick.SetActive(true);
    }
    public void CanGoToNextLevel()
    {
        nextLevelButton.SetActive(true);
        pointingArrow.SetActive(true);
    }
    public void NextLevelButton()
    {
        int sceneNumber = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(sceneNumber + 1);
    }
}
