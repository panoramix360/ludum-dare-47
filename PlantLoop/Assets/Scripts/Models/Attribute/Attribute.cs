using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class Attribute
{
    [SerializeField] public double baseValue;
    [SerializeField] public double value;
    [SerializeField] public double modifier;

    public Attribute(double baseValue, double value)
    {
        this.baseValue = baseValue;
        this.value = value;
    }

    public void IncrementValue(double value)
    {
        if (baseValue > this.value)
        {
            this.value += value;
            this.value = Math.Min(this.value, baseValue);
        }
    }

    public void UpgradeBaseValue(double value)
    {
        this.baseValue += value;
    }
}
