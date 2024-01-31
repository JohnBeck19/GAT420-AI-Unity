using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateMachine
{
    private Dictionary<string, AIState> states = new Dictionary<string, AIState>();
    public AIState CurrentState { get; private set; } = null;

    public void Update()
    {
        CurrentState?.OnUpdate();
    }

    public void AddState(string name, AIState state)
    {
        Debug.Assert(!states.ContainsKey(name),"State machine already contains state " + name);

        states[name] = state;
    }
    public void setState(string name)
    {
        Debug.Assert(states.ContainsKey(name), "State machine does not contains state " + name);
        
        AIState newState = states[name];

        //dont re-enter state 
        if (newState == CurrentState) return;

        //exit current state
        CurrentState?.OnExit();
        //set new state
        CurrentState = newState;
        //enter new state 
        CurrentState?.OnEnter();
    }
}
