using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnergyAttribute : Attribute
{
    public EnergyAttribute(float baseValue, float value, float unitPerTime) : base(baseValue, value, unitPerTime)
    {
        this.upgradeCosts.Add(new UpgradeCost(20, AttributeEnum.WATER));
    }
}
