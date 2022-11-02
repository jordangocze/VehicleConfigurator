using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    protected StateMachine _context;
    
    public State(StateMachine context)
    {
        _context = context;
    }

    public abstract void OnEnter();

    public abstract void LateUpdate();

    public abstract void OnExit();

    protected void TransitTo(State newState)
    {
        OnExit();

        newState.OnEnter();

        _context.CurrentState = newState;

        
    }
}
