using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WaterAttribute : Attribute
{
    public WaterAttribute(float baseValue, float value, float unitPerTime) : base(baseValue, value, unitPerTime)
    {
        this.upgradeCosts.Add(new UpgradeCost(30, AttributeEnum.ENERGY));
    }
}
