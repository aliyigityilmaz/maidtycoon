using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CustomerBehaviour : MonoBehaviour
{
    public static CustomerBehaviour instance;

    public GameObject[] chairPoints; 
    public GameObject[] alreadySitting;

    private GameObject randomChair;

    NavMeshAgent agent;


    public bool isSearching = false;

    public bool isSitting = false;
    public bool isOrdered = false;

    public bool waitingOrder = false;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        chairPoints = GameObject.FindGameObjectsWithTag("Sit");
        isSearching = true;
        waitingOrder = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isSitting && isSearching)
        {
            Sit();
        }

        if (!isSitting && !isSearching)
        {
            if (Vector3.Distance(randomChair.transform.position, this.transform.position) < 1)
            {
                Order();
                isSitting = true;
                GameManager.instance.AddToList(this.gameObject);
            }
        }
        

    }

    public void Sit()
    {
        foreach (GameObject sitPosition in chairPoints)
        {
            if (sitPosition != null && sitPosition.GetComponent<Chair>().isOccupied == false)
            {
                randomChair = sitPosition;
                isSearching = false;
                agent.SetDestination(sitPosition.transform.position);
                sitPosition.GetComponent<Chair>().SitChair();
                break;
            }
            else
            {
                StartCoroutine(Wait());
            }
        }

        
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(5);
    }

    public void Leave()
    {
        
        if (randomChair != null && randomChair.GetComponent<Chair>().isOccupied == true)
        {
            randomChair.GetComponent<Chair>().LeaveChair();
            isSitting = false;
        }
        
    }

    public void Order()
    {
        Debug.Log("Ordering");
        if (!waitingOrder)
        {
            //OrderSystem.instance.NewOrder();
            waitingOrder = true;
        }
    }

    public void ServeOrder()
    {
        Debug.Log("Order Served");
        GameManager.instance.seatedCustomer.Remove(this.gameObject);
        Eat();
    }


    public void Eat()
    {
        Debug.Log("Eating");
        StartCoroutine(Eating());
    }

    IEnumerator Eating()
    {
        yield return new WaitForSeconds(5);
        LeaveRestaurant();
    }

    public void LeaveRestaurant()
    {
        Leave();
        GameManager.instance.seatedCustomer.Remove(this.gameObject);
        Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        CustomerManager.instance.currentCustomers--;
        Leave();
    }
}
