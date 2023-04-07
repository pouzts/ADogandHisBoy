using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Predicate
{
    Less,
    LessOrEqual,
    Equal,
    GreaterOrEqual,
    Greater
}

public abstract class Condition 
{ 
    public abstract bool IsTrue();
}

public class Condition<T> : Condition where T : struct, IComparable<T>
{
    private readonly RefValue<T> refValue;
    private readonly T value;
    private readonly Predicate predicate;

    public Condition(RefValue<T> refValue, T value, Predicate predicate)
    {
        this.refValue = refValue;
        this.value = value;
        this.predicate = predicate;
    }

    public override bool IsTrue() 
    {
        switch (predicate)
        {
            case Predicate.Less:
                return refValue.Value.CompareTo(value) < 0;
            case Predicate.LessOrEqual:
                return refValue.Value.CompareTo(value) <= 0;
            case Predicate.Equal:
                return refValue.Value.CompareTo(value) == 0;
            case Predicate.GreaterOrEqual:
                return refValue.Value.CompareTo(value) >= 0;
            case Predicate.Greater:
                return refValue.Value.CompareTo(value) > 0;
            default:
                return false;
        }
    }
}
