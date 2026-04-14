using System;
using UnityEngine;

public static class DelegateTypes
{
    public delegate void Void();
    public delegate void Void_Bool(bool value); // returnTypes_parameterTypes eg Float_2Floats
    public delegate void Void_object(object value);
}
