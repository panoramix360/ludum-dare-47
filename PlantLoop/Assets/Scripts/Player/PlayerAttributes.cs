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
        GameController.Instance.UpdatePlayerAttributesUI();
    }

    private void SetupPlayerAttributes()
    {
        structure = new StructureAttribute(baseStructure, initialStructure, initialStructurePerSec);
        water = new WaterAttribute(baseWater, initialWater, initialWaterPerSec);
        energy = new EnergyAttribute(baseEnergy, initialEnergy, initialEnergyPerSec);
    }

    public void IncrementAttributesByValue(float totalStructureModifiers, float totalWaterModifiers, float totalEnergyModifiers)
    {
        structure.IncrementValue(totalStructureModifiers);
        water.IncrementValue(totalWaterModifiers);
        energy.IncrementValue(totalEnergyModifiers);
    }

    public bool CanPayAttributes(List<AttributeModifier> costs)
    {
        foreach (AttributeModifier attrModifier in costs)
        {
            switch (attrModifier.attr)
            {
                case AttributeEnum.ENERGY:
                    return energy.value >= attrModifier.value;
                case AttributeEnum.WATER:
                    return water.value >= attrModifier.value;
                case AttributeEnum.STRUCTURE:
                    return structure.value >= attrModifier.value;
            }
        }

        return true;
    }

    public void PayAttributesCost(List<AttributeModifier> attrsModifiers)
    {
        float totalStructureToDecrement = 0;
        float totalWaterToDecrement = 0;
        float totalEnergyToDecrement = 0;

        foreach (AttributeModifier attrModifier in attrsModifiers)
        {
            switch (attrModifier.attr)
            {
                case AttributeEnum.ENERGY:
                    totalEnergyToDecrement = attrModifier.value;
                    break;
                case AttributeEnum.WATER:
                    totalWaterToDecrement = attrModifier.value;
                    break;
                case AttributeEnum.STRUCTURE:
                    totalStructureToDecrement = attrModifier.value;
                    break;
            }
        }

        structure.DecrementValue(totalStructureToDecrement);
        water.DecrementValue(totalWaterToDecrement);
        energy.DecrementValue(totalEnergyToDecrement);
    }
}
