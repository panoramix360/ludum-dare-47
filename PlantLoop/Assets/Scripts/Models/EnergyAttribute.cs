using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyAttribute : Attribute
{
    public EnergyAttribute(double baseValue, double value) : base(baseValue, value) { }
}
