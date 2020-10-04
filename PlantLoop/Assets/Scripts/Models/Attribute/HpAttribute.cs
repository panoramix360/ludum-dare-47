using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpAttribute : Attribute
{
    public HpAttribute(float baseValue, float value) : base(baseValue, value)
    {
        this.upgradeCosts.Add(new UpgradeCost(10, AttributeEnum.ENERGY));
        this.upgradeCosts.Add(new UpgradeCost(10, AttributeEnum.WATER));
        this.upgradeCosts.Add(new UpgradeCost(20, AttributeEnum.HP));
    }
}
