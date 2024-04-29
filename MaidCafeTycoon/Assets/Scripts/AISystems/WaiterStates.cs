using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaiterStates : MonoBehaviour
{
    NavMeshAgent agent;
    private bool hasFood = false;
    public GameObject counter;
    [SerializeField]
    private Transform carry;


    [SerializeField]
    private GameObject food;

    [Header("ServeCustomer")]
    [SerializeField]
    private GameObject assignedCustomer;
    [SerializeField]
    private bool foundaCustomer;
    [SerializeField]
    private bool tookOrder;

    [Header("CheckForState")]
    public bool isIdle;
    public bool isTakeOrder;
    public bool isTakeFood;
    public bool isGoCustomer;
    public bool isServe;


    private enum State
    {
        Idle,
        TakeOrder,
        TakeFood,
        GoCustomer,
        Serve
    }
    private State currentState;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentState = State.Idle;
    }


    void Update()
    {
        switch (currentState)
        {
            case State.Idle:
                Idle();
                isIdle = true;
                isTakeOrder = false;
                isTakeFood = false;
                isGoCustomer = false;
                isServe = false;
                break;
            case State.TakeOrder:
                TakeOrder();
                isIdle = false;
                isTakeOrder = true;
                isTakeFood = false;
                isGoCustomer = false;
                isServe = false;
                break;
            case State.TakeFood:
                TakeFood();
                isIdle = false;
                isTakeOrder = false;
                isTakeFood = true;
                isGoCustomer = false;
                isServe = false;
                break;
            case State.GoCustomer:
                GoCustomer();
                isIdle = false;
                isTakeOrder = false;
                isTakeFood = false;
                isGoCustomer = true;
                isServe = false;
                break;
            case State.Serve:
                Serve();
                isIdle = false;                   
                isTakeOrder = false;
                isTakeFood = false;
                isGoCustomer = false;
                isServe = true;
                break;
        }
    }

    private void TakeFood()
    {
        if (counter != null)
        {
            agent.SetDestination(counter.transform.position);
            if (Vector3.Distance(this.transform.position, counter.transform.position) < 0.5f)
            {
                food = GameObject.FindGameObjectWithTag("Food");
                food.transform.SetParent(carry);
                food.transform.position = carry.transform.position;
                OrderSystem.instance.OrderCompleted(food);
                hasFood = true;
                currentState = State.Serve;
            }
        }
    }

    private void Serve()
    {
        int id = food.GetComponent<Food>().id;
        foreach (GameObject customer in GameManager.instance.seatedCustomer)
        {
            if (customer != null)
            {
                if (customer.GetComponent<CustomerStates>().customerID == id)
                {
                    assignedCustomer = customer;
                    break;
                }
            }
        }
        agent.SetDestination(assignedCustomer.transform.position);
        if (Vector3.Distance(this.transform.position, assignedCustomer.transform.position) < 1.5f)
        {
            assignedCustomer.GetComponent<CustomerStates>().Eat();
            assignedCustomer = null;
            hasFood = false;
            Destroy(food);
            food = null;
            currentState = State.Idle;
        }
    }

    private void GoCustomer()
    {
        assignedCustomer = OrderSystem.instance.waitingToOrder[0];
        if (assignedCustomer == null)
        {
            currentState = State.Idle;
            return;
        }
        else
        {
            assignedCustomer.GetComponent<CustomerStates>().attendWaiter(this.gameObject);
        }
        if (!foundaCustomer && assignedCustomer)
        {
            agent.SetDestination(assignedCustomer.transform.position);
            foundaCustomer = true;
        }
        else if (foundaCustomer)
        {
            if (Vector3.Distance(this.transform.position, assignedCustomer.transform.position) < 1.5f)
            {
                Debug.Log("Arrived at customer");
                agent.SetDestination(this.gameObject.transform.position);
                tookOrder = false;
                currentState = State.TakeOrder;               
            }
        }
    }

    private void TakeOrder()
    {
        if (!tookOrder)
        {
            StartCoroutine(Wait());
        }
        if (assignedCustomer==null)
        {
            currentState = State.Idle;
        }
    }

    IEnumerator Wait()
    {
        Debug.Log("Taking Order");
        tookOrder = true;
        yield return new WaitForSeconds(5);
        currentState = State.TakeOrder;
        assignedCustomer.GetComponent<CustomerStates>().Order();
        OrderSystem.instance.RemoveFromWaitingList(assignedCustomer);
        assignedCustomer.GetComponent<CustomerStates>().leaveWaiter();
        assignedCustomer = null;
    }

    private void Idle()
    {
        foundaCustomer = false;
        agent.SetDestination(this.transform.position);
        if (agent != null)
        {
            if (OrderSystem.instance.waitingToOrder.Count > 0 && CustomerStates.instance.waiterAttended == false)
            {
                currentState = State.GoCustomer;
            }
            else if (OrderSystem.instance.activeOrders.Count > 0)
            { 
                currentState = State.TakeFood;
            }
            else 
            { 
                currentState = State.Idle;
            }
        }
    }
}
