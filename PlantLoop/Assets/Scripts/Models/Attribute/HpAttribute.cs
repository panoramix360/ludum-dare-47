using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HpAttribute : Attribute
{
    public HpAttribute(float baseValue, float value, float unitPerTime) : base(baseValue, value, unitPerTime)
    {
        this.upgradeCosts.Add(new UpgradeCost(10, AttributeEnum.ENERGY));
        this.upgradeCosts.Add(new UpgradeCost(10, AttributeEnum.WATER));
        this.upgradeCosts.Add(new UpgradeCost(20, AttributeEnum.HP));
    }
}
