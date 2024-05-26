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
    public GameObject assignedCustomer;
    public bool foundaCustomer;
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
    private float fillDuration = 2f;
    private float elapsedTime;
    private Camera camera;


    private Animator Animator;
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
        Animator = GetComponent<Animator>();
        camera = Camera.main;
        counter = GameObject.FindGameObjectWithTag("FoodCounter");
        carry = transform.GetChild(0);
    }


    void Update()
    {
        agent.acceleration = agentSpeed;
        agent.speed = agentSpeed;
        progressBar.transform.LookAt(transform.position + camera.transform.rotation * Vector3.forward, camera.transform.rotation * Vector3.up);
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
            Animator.SetBool("isWalking", true);
            Animator.SetBool("isCarrying", false);
            agent.SetDestination(counter.transform.position);
            if (Vector3.Distance(this.transform.position, counter.transform.position) < 1f && hasFood == false)
            {
                GameObject[] foods = GameObject.FindGameObjectsWithTag("Food");
                foreach (GameObject carriedfood in foods)
                {
                    if (carriedfood.GetComponent<Food>().taken == false && carriedfood != null)
                    {
                        carriedfood.GetComponent<Food>().taken = true; // Alýnan yemeði hemen iþaretle
                        carriedfood.transform.SetParent(carry);
                        carriedfood.transform.position = carry.transform.position;
                        hasFood = true;
                        food = carriedfood;
                        OrderSystem.instance.OrderCompleted(food.gameObject);
                        DayLoop.instance.AddSold();
                        Animator.SetBool("isCarrying", true);
                        Animator.SetBool("isWalking", false);
                        currentState = State.Serve;
                        break;
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
        if (Vector3.Distance(this.transform.position, assignedCustomer.transform.position) < 2f)
        {
            assignedCustomer.GetComponent<CustomerStates>().Eat();
            Animator.SetBool("isCarrying", false);
            Animator.SetBool("isWalking", true);
            CheckList.instance.soldFoodCount++;
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
                            Animator.SetBool("isCarrying", false);
                            Animator.SetBool("isWalking", true);
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
            
            if (Vector3.Distance(this.transform.position, assignedCustomer.transform.position) < 2f)
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
        Animator.SetBool("isWalking", false);
        Animator.SetBool("isCarrying", false);
        while (elapsedTime < fillDuration)
        {
            elapsedTime += Time.deltaTime;
            progressBar.fillAmount = Mathf.Clamp01(elapsedTime / fillDuration);
            yield return null;
        }
        progressBar.fillAmount = 1f;
        assignedCustomer.GetComponent<CustomerStates>().Order();
        assignedCustomer.GetComponent<CustomerStates>().leaveWaiter();
        assignedCustomer = null;
        currentState = State.Idle;
    }

    private void Idle()
    {
        Animator.SetBool("isWalking", false);
        Animator.SetBool("isCarrying", false);
        progressBar.fillAmount = 0f;
        elapsedTime = 0;
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
                ResetUI();
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

    public void SetStateIdle()
    {
        currentState = State.Idle;
    }
}
