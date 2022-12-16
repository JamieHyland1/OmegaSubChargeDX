using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class StateMachine : MonoBehaviour
{
    public IState currentState;
    public IState previousState;

    bool _inTransition = false;

    public void ChangeState(IState newState)
    {
        if (newState == currentState) return;
        ChangeStateRoutine(newState);
    }

    public void RevertState()
    {
        if (previousState != null || _inTransition) ChangeState(previousState);
    }

    void ChangeStateRoutine(IState newState)
    {
        _inTransition = true;

        if (currentState != null)
            currentState.Exit();

        if (previousState != null)
            previousState = currentState;

        currentState = newState;

        if (currentState != null)
            currentState.Enter();

        _inTransition = false;
    }

    public void Update()
    {
        if (currentState != null && !_inTransition)
            currentState.Tick();
    }
    public void FixedUpdate()
    {
        if (currentState != null && !_inTransition)
            currentState.FixedTick();
    }
}