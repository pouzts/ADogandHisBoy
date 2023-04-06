using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefValue<T> where T : struct
{
    public T Value { get; private set; }

    public static implicit operator T(RefValue<T> refValue) { return refValue.Value; }
}
