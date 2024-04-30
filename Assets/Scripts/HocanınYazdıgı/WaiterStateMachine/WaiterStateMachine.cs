using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaiterStateMachine : BaseStateMachine
{


    private enum State
    {
        Idle,
        TakeOrder,
        TakeFood,
        GoCustomer,
        Serve
    }

    private BaseState currentState;

    #region States
    IdleState idleState = new IdleState();
    TakeOrderState orderState = new TakeOrderState();
    TakeFoodState takeFoodState = new TakeFoodState();
    GoCustamerState goCustomerState = new GoCustamerState();
    ServeState serveState = new ServeState();
    #endregion States



    void Start()
    {
        currentState = idleState;
        currentState.StartState();
    }

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
