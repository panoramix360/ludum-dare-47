using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyAttribute : Attribute
{
    public EnergyAttribute(float baseValue, float value) : base(baseValue, value)
    {
        this.upgradeCosts.Add(new UpgradeCost(20, AttributeEnum.WATER));
    }
}
