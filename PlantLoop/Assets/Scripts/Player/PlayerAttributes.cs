using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttributes : MonoBehaviour
{
    [Header("Initial Bases")]
    [SerializeField] private float baseHp;
    [SerializeField] private float baseWater;
    [SerializeField] private float baseEnergy;

    [Header("Initial stats")]
    [SerializeField] private float initialHp;
    [SerializeField] private float initialWater;
    [SerializeField] private float initialEnergy;

    [Header("Initial PerSec")]
    [SerializeField] private float initialHpPerSec;
    [SerializeField] private float initialWaterPerSec;
    [SerializeField] private float initialEnergyPerSec;

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
        hp = new HpAttribute(baseHp, initialHp, initialHpPerSec);
        water = new WaterAttribute(baseWater, initialWater, initialWaterPerSec);
        energy = new EnergyAttribute(baseEnergy, initialEnergy, initialEnergyPerSec);
    }
}
