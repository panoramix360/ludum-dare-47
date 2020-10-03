using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Environment
{
    public double HpModifier;
    public double WaterModifier;
    public double EnergyModifier;
    public EnvironmentType Type;

    public Environment(EnvironmentType type)
    {
        switch (type)
        {
            case EnvironmentType.FOREST:
                Forest();
                break;
            case EnvironmentType.SWAMP:
                Swamp();
                break;
            case EnvironmentType.DESERT:
                Desert();
                break;
            default:
                Forest();
                break;
        }
    }

    private void Forest()
    {
        Type = EnvironmentType.FOREST;
        this.HpModifier = 0.5;
        this.WaterModifier = 1.5;
        this.EnergyModifier = 1;
    }
    private void Swamp()
    {
        Type = EnvironmentType.SWAMP;
        this.HpModifier = 0.3;
        this.WaterModifier = 2;
        this.EnergyModifier = 0.5;
    }
    private void Desert()
    {
        Type = EnvironmentType.DESERT;
        this.HpModifier = 0.1;
        this.WaterModifier = 0.5;
        this.EnergyModifier = 2;
    }
}
