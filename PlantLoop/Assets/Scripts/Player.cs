using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Bases")]
    [SerializeField] private double baseHp;
    [SerializeField] private double baseWater;
    [SerializeField] private double baseEnergy;

    [Header("Player Attributes")]
    [SerializeField] private double initialHp;
    [SerializeField] private double initialWater;
    [SerializeField] private double initialEnergy;

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

    public void IncrementHpAttribute(double value)
    {
        hp.IncrementValue(value);
    }

    public void IncrementWaterAttribute(double value)
    {
        water.IncrementValue(value);
    }

    public void IncrementEnergyAttribute(double value)
    {
        energy.IncrementValue(value);
    }
}
