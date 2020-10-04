using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Attribute
{
    [SerializeField] public float baseValue;
    [SerializeField] public float value;
    [SerializeField] public float modifier;
    [SerializeField] public float unitPerTime;
    [SerializeField] public List<UpgradeCost> upgradeCosts;

    public Attribute(float baseValue, float value, float unitPerTime)
    {
        this.baseValue = baseValue;
        this.value = value;
        this.unitPerTime = unitPerTime;
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

    public void IncrementModifier(float modifierIncrement)
    {
        this.modifier = modifierIncrement;
    }

    public float GetUnitPerTime()
    {
        return this.unitPerTime + this.modifier;
    }

    public void IncrementBaseValue(float value)
    {
        this.baseValue += value;
    }

    public void IncrementUnitPerTime(float increment)
    {
        this.unitPerTime += increment;
    }
}
