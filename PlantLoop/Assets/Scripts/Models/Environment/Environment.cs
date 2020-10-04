using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment
{
    public float HpModifier;
    public float WaterModifier;
    public float EnergyModifier;
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
        this.HpModifier = 0.5f;
        this.WaterModifier = 1.5f;
        this.EnergyModifier = 1f;
    }
    private void Swamp()
    {
        Type = EnvironmentType.SWAMP;
        this.HpModifier = 0.3f;
        this.WaterModifier = 2f;
        this.EnergyModifier = 0.5f;
    }
    private void Desert()
    {
        Type = EnvironmentType.DESERT;
        this.HpModifier = 0.1f;
        this.WaterModifier = 0.5f;
        this.EnergyModifier = 2f;
    }
}
