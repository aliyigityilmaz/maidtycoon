using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Food")]
public class FoodSO : ScriptableObject
{
    public int foodID;
    public string foodName;
    public int foodValue;
    public float foodTimer;
    
}
