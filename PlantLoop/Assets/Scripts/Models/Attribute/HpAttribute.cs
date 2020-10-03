using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpAttribute : Attribute
{
    public HpAttribute(double baseValue, double value) : base(baseValue, value) {}
}
