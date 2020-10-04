using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttributes : MonoBehaviour
{
    [Header("Bases")]
    [SerializeField] private float baseHp;
    [SerializeField] private float baseWater;
    [SerializeField] private float baseEnergy;

    [Header("Player Attributes")]
    [SerializeField] private float initialHp;
    [SerializeField] private float initialWater;
    [SerializeField] private float initialEnergy;

    [SerializeField] public Attribute hp;
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
        hp = new HpAttribute(baseHp, initialHp);
        water = new WaterAttribute(baseWater, initialWater);
        energy = new EnergyAttribute(baseEnergy, initialEnergy);
    }

    public void IncrementHpAttribute(float value)
    {
        hp.IncrementValue(value);
    }

    public void IncrementWaterAttribute(float value)
    {
        water.IncrementValue(value);
    }

    public void IncrementEnergyAttribute(float value)
    {
        energy.IncrementValue(value);
    }

    public void DecrementAttribute(Attribute attribute, float value)
    {
        attribute.DecrementValue(value);
    }
}
