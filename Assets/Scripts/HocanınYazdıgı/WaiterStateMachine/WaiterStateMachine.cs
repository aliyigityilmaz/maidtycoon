using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    public void ChangingState(BaseState state)
    {
        this.currentState = state;
        this.currentState.StartState();
    }

    public override void ChangeState(BaseState state)
    {
        throw new System.NotImplementedException();
    }
}
