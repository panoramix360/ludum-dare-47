using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpAttribute : Attribute
{
    public HpAttribute(float baseValue, float value) : base(baseValue, value) {}
}
