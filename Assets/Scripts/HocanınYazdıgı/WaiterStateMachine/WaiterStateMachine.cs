using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaiterStateMachine : MonoBehaviour
{

    private BaseState currentState;

    #region States
    IdleState idleState = new IdleState();
    #endregion States



    void Start()
    {
        currentState = idleState;
        currentState.StartState();
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState();
    }

    public void ChangeState(BaseState state)
    {
        this.currentState = state;
        this.currentState.StartState();
    }
}
