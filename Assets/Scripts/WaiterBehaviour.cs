using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

public class WaiterBehaviour : MonoBehaviour
{
    public GameObject[] customers;

    public GameObject counter;
    public GameObject customerLocation;
    public GameObject foodPrefab;

    public Transform carry;

    NavMeshAgent agent;

    private bool hasFood = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

    }

    void Update()
    {
        
        if (counter != null)
        {
            if(OrderSystem.instance.activeOrders.Count > 0)
            {
                agent.SetDestination(counter.transform.position);
                if (Vector3.Distance(this.transform.position, counter.transform.position) < 0.5f)
                {
                    TakeOrder();
                }
            }
        }

        if(hasFood)
        {
            GoCustomer();
            Serve();
        }
    }


    public void GoCustomer()
    {
        if (hasFood)
        {
            if (GameManager.instance.seatedCustomer.Count>0)
            {
                GameObject customer = GameManager.instance.seatedCustomer[0];
                agent.SetDestination(customer.transform.position);
                customerLocation = customer;
            }
        }
    }


    public void TakeOrder()
    {
       if (counter != null)
       {
            if (Vector3.Distance(this.transform.position, counter.transform.position) < 0.5f)
            {
                foodPrefab = GameObject.FindGameObjectWithTag("Food");
                foodPrefab.transform.SetParent(carry);
                foodPrefab.transform.position = carry.transform.position;
                hasFood = true;
            }
       }
    }


    public void Serve()
    {
        if (Vector3.Distance(this.transform.position, customerLocation.transform.position) < 2f)
        {
            OrderSystem.instance.OrderCompleted(foodPrefab);
            foodPrefab.transform.SetParent(null);
            customerLocation.GetComponent<CustomerBehaviour>().ServeOrder();
            Destroy(foodPrefab);
            hasFood = false;
        }
    }
}
