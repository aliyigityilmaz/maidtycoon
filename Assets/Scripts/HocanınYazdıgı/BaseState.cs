using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class BaseState
{
    public abstract void StartState();
    public abstract void UpdateState();

    public abstract void LeaveState();

}
