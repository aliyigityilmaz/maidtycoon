using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class BaseState
{
    public abstract void StartState(BaseStateMachine machine);
    public abstract void UpdateState(BaseStateMachine machine);

    public abstract void LeaveState(BaseStateMachine machine);

    internal void StartState()
    {
        throw new NotImplementedException();
    }

    internal void UpdateState()
    {
        throw new NotImplementedException();
    }
}
