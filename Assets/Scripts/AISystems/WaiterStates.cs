using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class WaiterStates : MonoBehaviour
{
    public static WaiterStates instance;

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

    public float agentSpeed;

    [Header("UI")]
    [SerializeField] private Image progressBar;
    public float orderTimer;
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
        instance = this;
        
    }


    void Update()
    {
        agent.acceleration = agentSpeed;
        agent.speed = agentSpeed;
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
            if (Vector3.Distance(this.transform.position, counter.transform.position) < 0.5f && hasFood == false)
            {
                 GameObject[] foods = GameObject.FindGameObjectsWithTag("Food");
                foreach (GameObject carriedfood in foods)
                {
                    if (carriedfood.GetComponent<Food>().taken == false)
                    {
                        carriedfood.transform.SetParent(carry);
                        carriedfood.transform.position = carry.transform.position;
                        hasFood = true;
                        food = carriedfood;
                        OrderSystem.instance.OrderDelivered(food.gameObject);
                        currentState = State.Serve;
                    }
                }
            }
        }
        if (hasFood)
        {
            currentState = State.Serve;
        }
    }

    private void Serve()
    {
        if (food == null)
        {
            currentState = State.Idle;
            return;
        }
        food.GetComponent<Food>().taken = true;
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
            EconomyManager.instance.AddMoney(food.GetComponent<Food>().foodso.foodValue);
            Destroy(food);
            food = null;
            currentState = State.Idle;
        }
    }

    private void GoCustomer()
    {
        GameObject[] customers = GameObject.FindGameObjectsWithTag("Customer");
        if (assignedCustomer == false)
        {
            foreach (GameObject customer in customers)
            {
                if (customer != null)
                {
                    if (OrderSystem.instance.waitingToOrder.Contains(customer))
                    {
                        if (customer.GetComponent<CustomerStates>().waiterAttended == false)
                        {
                            assignedCustomer = customer;
                            foundaCustomer = true;
                            assignedCustomer.GetComponent<CustomerStates>().attendWaiter(this.gameObject);

                            OrderSystem.instance.RemoveFromWaitingList(assignedCustomer);
                            break;
                        }
                        else
                        {
                            currentState = State.Idle;
                        }
                    }
                }
                else
                {
                    currentState = State.Idle;
                }
            }
        }
        if (foundaCustomer)
        {
            agent.SetDestination(assignedCustomer.transform.position);
            if (Vector3.Distance(this.transform.position, assignedCustomer.transform.position) < 1.5f)
            {
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
        tookOrder = true;
        yield return new WaitForSeconds(orderTimer);
        UpdateProgressBar(orderTimer * Time.deltaTime);
        assignedCustomer.GetComponent<CustomerStates>().Order();
        assignedCustomer.GetComponent<CustomerStates>().leaveWaiter();
        assignedCustomer = null;
        currentState = State.Idle;
    }

    private void Idle()
    {
        foundaCustomer = false;
        agent.SetDestination(this.transform.position);
        if (agent != null)
        {
            if (OrderSystem.instance.waitingToOrder.Count > 0)
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
                ResetUI();
            }
        }
    }

    private bool IsCustomerEmpty()
    {
        GameObject[] customers = GameObject.FindGameObjectsWithTag("Customer");
        foreach (GameObject customer in customers)
        {
            if (customer.GetComponent<CustomerStates>().waiterAttended == false)
            {
                return true;
                
            }
            else
            {
                return false;
            }
        }
        return false;
    }
    public void UpdateProgressBar(float fillAmount)
    {
        
        progressBar.fillAmount = fillAmount;
    }
    public void ResetUI()
    {
        progressBar.fillAmount = 0f;
    }
}
