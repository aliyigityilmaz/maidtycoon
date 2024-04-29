using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chair : MonoBehaviour
{
    public static Chair instance;
    public bool isOccupied = false;

    private void Awake()
    {
        instance = this;
    }

    public void SitChair()
    {
        isOccupied = true;
    }
    public void LeaveChair()
    {
        isOccupied = false;
    }


}
