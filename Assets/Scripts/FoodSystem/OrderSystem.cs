using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class OrderSystem : MonoBehaviour
{
    public static OrderSystem instance;
    public GameObject[] orderList;

    public GameObject counter;

    public List<GameObject> waitingToOrder;

    public List<GameObject> activeOrders = new List<GameObject>();

    private int currentOrderId = 0; // Benzersiz ID oluþturmak için sayaç

    [Header("Bools")]
    public bool firstUpgrade;
    public float firstUpgradeMultiplier;
    public bool secondUpgrade;
    public float secondUpgradeMultiplier;
    public bool thirdUpgrade;
    public float thirdUpgradeMultiplier;


    private void Awake()
    {
        instance = this;
    }

    public void NewOrder(int id)
    {
        int randomFood = Random.Range(0, orderList.Length);
        Debug.Log("New Order: " + orderList[randomFood].name);

        // Yeni bir yemek oluþtur
        GameObject food = Instantiate(orderList[randomFood], counter.transform.position, Quaternion.identity);

        //

        food.GetComponent<Food>().value = Mathf.RoundToInt(food.GetComponent<Food>().value * 
            (thirdUpgrade ? thirdUpgradeMultiplier : 
            secondUpgrade ? secondUpgradeMultiplier : 
            firstUpgrade ? firstUpgradeMultiplier : 1));



        //



        // Yemeðe benzersiz bir ID ata
        food.GetComponent<Food>().id = id;
        food.GetComponent<Food>().id = currentOrderId++;

        // Yemek objesini activeOrders listesine ekle
        activeOrders.Add(food);
    }

    public void OrderCompleted(GameObject ordered)
    {
        GameObject orderToRemove = null;

        foreach (GameObject order in activeOrders)
        {
            if (order.GetComponent<Food>().id == ordered.GetComponent<Food>().id)
            {
                orderToRemove = order;
                break;
            }
        }

        if (orderToRemove != null)
        {
            activeOrders.Remove(orderToRemove);
            Debug.Log("Order completed and removed: " + ordered.GetComponent<Food>().name);
        }
        else
        {
            Debug.Log("Order not found in active orders.");
        }
    }

    public GameObject AddWaitingList(GameObject customer)
    {
        waitingToOrder.Add(customer);
        return customer;
    }

    public void RemoveFromWaitingList(GameObject customer)
    {
        Debug.Log("Removing from waiting list");
        waitingToOrder.Remove(customer);
    }



}