﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterAttribute : Attribute
{
    public WaterAttribute(float baseValue, float value) : base(baseValue, value) { }
}
