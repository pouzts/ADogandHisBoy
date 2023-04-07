using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public Dictionary<State, KeyValuePair<Transition, State>> stateTransitions;
    public State CurrentState { get; private set; }

    void Update()
    {
        if (CurrentState != null)
            return;

        var transitions = stateTransitions[CurrentState];

        CurrentState.OnUpdate();
    }

    public void SetState(State state) 
    {
        if (CurrentState == state || state == null)
            return;

        CurrentState?.OnExit();
        CurrentState = state;
        CurrentState.OnEnter();
    }

    public State GetState(string name) 
    {
        return null;
    }

    public string GetStateName()
    { 
        return CurrentState?.Name;
    }
}
