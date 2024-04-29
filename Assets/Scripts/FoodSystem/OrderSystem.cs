using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.Events;

public class OrderSystem : MonoBehaviour
{
    public static OrderSystem instance;
    public GameObject[] orderList;

    public GameObject counter;

    public List<GameObject> waitingToOrder;

    public List<GameObject> activeOrders = new List<GameObject>();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
    }

    private void Update()
    {
        
    }
    public void NewOrder(int id)
    {
        int randomFood = Random.Range(0, orderList.Length);
        Debug.Log("New Order: " + orderList[randomFood].name);
        GameObject food = Instantiate(orderList[randomFood], counter.transform.position, Quaternion.identity);
        activeOrders.Add(food);
        food.GetComponent<Food>().id= id;        
    }

    public void OrderCompleted(GameObject ordered)
    {
        activeOrders.Remove(ordered);
        Debug.Log("Order Completed: " + ordered.name);
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
