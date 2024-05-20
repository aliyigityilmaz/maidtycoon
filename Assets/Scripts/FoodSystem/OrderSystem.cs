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

    private int orderIDCounter = 0;

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
        GameObject food = orderList[randomFood].gameObject;
        food.GetComponent<Food>().id= id;        
        food.GetComponent<Food>().orderID = GetUniqueID();
        StartCoroutine(OrderTimer(food.GetComponent<Food>().foodso.foodTimer, food));
    }

    private int GetUniqueID()
    {
        return orderIDCounter++;
    }

    IEnumerator OrderTimer(float time, GameObject ordered)
    {
        yield return new WaitForSeconds(time);
        OrderCompleted(ordered);
    }

    public void OrderCompleted(GameObject ordered)
    {
        activeOrders.Add(ordered);
        GameObject instantiatedOrder = Instantiate(ordered, counter.transform.position, Quaternion.identity);
        instantiatedOrder.GetComponent<Food>().orderID = ordered.GetComponent<Food>().orderID;
    }

    public void OrderDelivered(GameObject delivered)
    {
        int deliveredID = delivered.GetComponent<Food>().orderID;
        GameObject order = activeOrders.Find(x => x.GetComponent<Food>().orderID == deliveredID);
        activeOrders.Remove(order);
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
