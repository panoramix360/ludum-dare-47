using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class Attribute
{
    [SerializeField] public float baseValue;
    [SerializeField] public float value;
    [SerializeField] public float modifier;
    [SerializeField] public List<UpgradeCost> upgradeCosts;

    public Attribute(float baseValue, float value)
    {
        this.baseValue = baseValue;
        this.value = value;
        this.upgradeCosts = new List<UpgradeCost>();
    }

    public void IncrementValue(float value)
    {
        if (baseValue > this.value)
        {
            this.value += value;
            this.value = Math.Min(this.value, baseValue);
        }
    }

    public void DecrementValue(float value)
    {
        this.value -= value;
        this.value = Math.Max(this.value, 0);
    }

    public void UpgradeBaseValue(float value)
    {
        this.baseValue += value;
    }
}
