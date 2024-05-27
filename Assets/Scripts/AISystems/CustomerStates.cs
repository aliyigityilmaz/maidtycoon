using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.Rendering.DebugUI;

public class CustomerStates : MonoBehaviour
{
    public static CustomerStates instance;
    NavMeshAgent agent;
    private Animator anim;

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
    GameObject closestTable = null;

    [Header("Waiting For Food")]
    public GameObject[] vfxs;
    public float waitingTimer;
    private float timer = 0;


    [Header("VFX")]
    public GameObject[] happyVfx;
    public GameObject[] angryVfx;


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
        anim = GetComponent<Animator>();
        timer = waitingTimer;
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
        if (currentState == State.WaitingOrder)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                DisplayRandomAngryVFX();
                AudioManager.instance.PlayRandomVFX(false);
                currentState = State.Leaving;
            }
        }
        else
        {
            timer = waitingTimer;
        }

        if(anim.GetBool("isSitting") == true)
        {
            transform.position = randomChair.transform.position;
        }
    }


    private void Searching()
    {
        randomChair = GameManager.instance.GetRandomChair();
        currentState = State.GoingToChair;
    }

    private void GoingToChair()
    {
        anim.SetBool("isWalking", true);
        anim.SetBool("isSitting", false);
        agent.SetDestination(randomChair.transform.position);
        if (Vector3.Distance(this.transform.position, randomChair.transform.position) < 1)
        {
            currentState = State.WaitingOrder;
        }

    }

    private void WaitingOrder()
    {
        FindClosestTable();
        anim.SetBool("isWalking", false);
        anim.SetBool("isSitting", true);
        Vector3 direction = closestTable.transform.position - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 0.1f);
        if (agent.enabled)
        {
            agent.enabled = false;
            var obstacle = gameObject.AddComponent<NavMeshObstacle>();
            obstacle.carving = true;
            obstacle.size = new Vector3(0.3f, 0.3f, 0.3f);
        }

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
        AudioManager.instance.PlayRandomVFX(true);
        DisplayRandomHappyVFX();
    }

    private void Eating()
    {
        StartCoroutine(EatingTimer());
        if (finishedEating)
        {
            gameObject.GetComponent<NavMeshObstacle>().enabled = false;
            agent.enabled = true;
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
        agent.enabled = true;
        gameObject.GetComponent<NavMeshObstacle>().enabled = false;
        OrderSystem.instance.RemoveFromWaitingList(this.gameObject);
        anim.SetBool("isWalking", true);
        anim.SetBool("isSitting", false);

        if (left)
        {
            GameManager.instance.RemoveChair(randomChair);
            left = false;
        }
        foreach (GameObject waiter in GameObject.FindGameObjectsWithTag("Waiter"))
        {
            if(waiter.GetComponent<WaiterStates>().assignedCustomer == this.gameObject)
            {
                waiter.GetComponent<WaiterStates>().assignedCustomer = null;
                waiter.GetComponent<WaiterStates>().foundaCustomer = false;
                if (waiter.GetComponent<WaiterStates>().food != null)
                {
                    waiter.GetComponent<WaiterStates>().food = null;
                }
                waiter.GetComponent<WaiterStates>().SetStateIdle();
            }
            else
            {
                Debug.Log("No waiter found");
            }
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

    private void FindClosestTable()
    {
        GameObject[] tables = GameObject.FindGameObjectsWithTag("Table");
        float minDistance = Mathf.Infinity;
        foreach (GameObject table in tables)
        {
            float distance = Vector3.Distance(this.transform.position, table.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestTable = table;
            }
        }
    }


    private void DisplayRandomAngryVFX()
    {
        int index = UnityEngine.Random.Range(0, angryVfx.Length);
        GameObject randomVfx = Instantiate(angryVfx[index], new Vector3(transform.position.x, transform.position.y + 3, transform.position.z), Quaternion.identity);
        Destroy(randomVfx, 5);
    }

    private void DisplayRandomHappyVFX()
    {
        int index = UnityEngine.Random.Range(0, happyVfx.Length);
        GameObject randomVfx = Instantiate(happyVfx[index], new Vector3(transform.position.x, transform.position.y + 3, transform.position.z), Quaternion.identity);
        Destroy(randomVfx, 5);
    }


}
