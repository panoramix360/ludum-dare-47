using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeCost
{
    public int cost;
    public AttributeEnum attributeType;

    public UpgradeCost(int cost, AttributeEnum attributeType)
    {
        this.cost = cost;
        this.attributeType = attributeType;
    }
}
