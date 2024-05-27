using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer.Internal;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    public static CustomerManager instance;
    public GameObject[] customerList;

    public int maxCustomers = 2;
    public int currentCustomers = 0;
    public float spawnRate = 1f;
    public float spawnTimer = 0f;

    public bool canSpawn = true;

    private int customerCounter;
    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentCustomers < maxCustomers && canSpawn)
        {
            spawnTimer += Time.deltaTime;
            if (spawnTimer >= spawnRate)
            {
                GameObject customer = NewCustomer();
                currentCustomers++;
                spawnTimer = 0;
            }
        }
    }

    public GameObject NewCustomer()
    {
        int randomCustomer = Random.Range(0, customerList.Length);
        GameObject newCustomer = Instantiate(customerList[randomCustomer], transform.position, Quaternion.identity);
        newCustomer.GetComponent<CustomerStates>().customerID = GetNextCustomerId();
        customerCounter++;
        return newCustomer;
    }
    public void IncreaseMaxCustomer(int customer)
    {
        maxCustomers += customer;
    }

    public int GetNextCustomerId()
    {
        return customerCounter;
    }
}
