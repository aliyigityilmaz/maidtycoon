using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CustomerStates : MonoBehaviour
{
    public static CustomerStates instance;
    NavMeshAgent agent;

    private GameObject randomChair;

    private GameObject exitDoor;

    [SerializeField]
    private GameObject attendedWaiter;
    public bool waiterAttended;


    public bool ReadyToOrder = false;
    public bool WaitingForFood = false;
    public bool FoodOrdered = false;
    public bool _Eating = false;
    public bool finishedEating = false;
    public bool _Leaving = false;

    public int customerID;

    private bool left;
    private enum State
    {
        Searching,
        GoingToChair,
        WaitingOrder,
        Ordering,
        WaitingFood,
        Eating,
        Leaving
    }
    private State currentState;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        left = true;
        agent = GetComponent<NavMeshAgent>();
        ReadyToOrder = false;
        currentState = State.Searching;
        exitDoor = GameObject.FindGameObjectWithTag("Exit");
    }

    void Update()
    {
        switch (currentState)
        {
            case State.Searching:
                Searching();
                break;
            case State.GoingToChair:
                GoingToChair();
                break;
            case State.WaitingOrder:
                WaitingOrder();
                break;
            case State.Ordering:
                Ordering();
                break;
            case State.WaitingFood:
                WaitingFood();
                break;
            case State.Eating:
                Eating();
                break;
            case State.Leaving:
                Leaving();
                break;
        }
        
    }
    private void Searching()
    {
        randomChair = GameManager.instance.GetRandomChair();
        currentState = State.GoingToChair;
    }

    private void GoingToChair()
    {
        agent.SetDestination(randomChair.transform.position);
        if (Vector3.Distance(this.transform.position, randomChair.transform.position) < 1)
        {
            currentState = State.WaitingOrder;
        }

    }

    private void WaitingOrder()
    { 
        if (!ReadyToOrder)
        {
            OrderSystem.instance.AddWaitingList(this.gameObject);
            ReadyToOrder = true;
        }
        if (WaitingForFood)
        {
            currentState = State.Ordering;
        }
    }

    public void Order()
    {        
        WaitingForFood = true;
    }

    private void Ordering()
    {
        if (!FoodOrdered)
        {
            Debug.Log("Ordering");
            OrderSystem.instance.NewOrder(customerID);
            GameManager.instance.seatedCustomer.Add(this.gameObject);
            FoodOrdered = true;
        }
        else if (FoodOrdered)
        {
            currentState = State.WaitingFood;
        }
    }

    private void WaitingFood()
    {
        if (_Eating)
        {
            currentState = State.Eating;
        }
        else
        {
            currentState = State.WaitingFood;
        }
    }

    public void Eat()
    {
        _Eating = true;
    }

    private void Eating()
    {
        StartCoroutine(EatingTimer());
        if (finishedEating)
        {
            _Leaving = true;
            currentState = State.Leaving;
        }
    }

    IEnumerator EatingTimer()
    {
        yield return new WaitForSeconds(5);
        finishedEating = true;
    }

    private void Leaving()
    {
        
        if (left)
        {
            GameManager.instance.RemoveChair(randomChair);
            left = false;
        }
        agent.SetDestination(exitDoor.transform.position);
        
        if (Vector3.Distance(this.transform.position, exitDoor.transform.position) < 1)
        {
            CustomerManager.instance.currentCustomers--;
            GameManager.instance.seatedCustomer.Remove(this.gameObject);
            Destroy(this.gameObject);
        }
    }

    public void attendWaiter(GameObject waiter)
    {
        attendedWaiter = waiter;
        waiterAttended = true;
    }

    public void leaveWaiter()
    {
        attendedWaiter = null;
        waiterAttended = false;
    }


}
