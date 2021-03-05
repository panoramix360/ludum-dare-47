using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttributes : MonoBehaviour
{
    [Header("Initial Bases")]
    [SerializeField] private float baseStructure;
    [SerializeField] private float baseWater;
    [SerializeField] private float baseEnergy;

    [Header("Initial stats")]
    [SerializeField] private float initialStructure;
    [SerializeField] private float initialWater;
    [SerializeField] private float initialEnergy;

    [Header("Initial PerSec")]
    [SerializeField] private float initialStructurePerSec;
    [SerializeField] private float initialWaterPerSec;
    [SerializeField] private float initialEnergyPerSec;

    [SerializeField] public Attribute structure;
    [SerializeField] public Attribute water;
    [SerializeField] public Attribute energy;

    private void Awake()
    {
        SetupPlayerAttributes();
    }

    private void Start()
    {
        GameController.Instance.UpdatePlayerAttributes();
    }

    private void SetupPlayerAttributes()
    {
        structure = new StructureAttribute(baseStructure, initialStructure, initialStructurePerSec);
        water = new WaterAttribute(baseWater, initialWater, initialWaterPerSec);
        energy = new EnergyAttribute(baseEnergy, initialEnergy, initialEnergyPerSec);
    }

    public void CheckIfAttributesAreBelow(float waterPerSec, float energyPerSec)
    {
        if (waterPerSec >= 0 || energyPerSec >= 0) return;

        float removePerSec = 0.0f;
        if (water.value <= 0)
        {
            removePerSec = waterPerSec;
        }

        if (energy.value <= 0)
        {
            removePerSec = energyPerSec;
        }

        structure.IncrementValue(removePerSec);
    }
}
