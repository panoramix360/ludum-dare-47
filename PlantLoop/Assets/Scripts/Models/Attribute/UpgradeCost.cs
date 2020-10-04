using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UpgradeCost
{
    [SerializeField] public int cost;
    [SerializeField] public AttributeEnum attributeType;

    public UpgradeCost(int cost, AttributeEnum attributeType)
    {
        this.cost = cost;
        this.attributeType = attributeType;
    }
}
