using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition<T> where T : struct, IComparable<T>
{
    private readonly State stateFrom;
    private readonly Condition<T>[] conditions;
    private readonly State stateTo;

    public Transition(State stateFrom, State stateTo, params Condition<T>[] conditions)
    { 
        this.stateFrom = stateFrom;
        this.conditions = conditions;
        this.stateTo = stateTo;
    }
}
