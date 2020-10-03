using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterAttribute : Attribute
{
    public WaterAttribute(double baseValue, double value) : base(baseValue, value) { }
}
