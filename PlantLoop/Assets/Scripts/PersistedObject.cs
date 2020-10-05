using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistedObject : Singleton<PersistedObject>
{
    public bool isChanged = false;

    public float baseHp;
    public float baseWater;
    public float baseEnergy;

    public float baseHpUnitPerTime;
    public float baseWaterUnitPerTime;
    public float baseEnergyUnitPerTime;

    public int currentEnvironment;
}
