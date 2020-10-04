using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent : MonoBehaviour
{
    System.Random Random;

    [Header("Game Event Types")]
    public GameEventType Type;
    public DangerEventType DangerType;
    public ClimateEventType ClimateType;
    public OtherEventType OtherType;

    [Header("Game Event Modifiers")]
    public float EnergyModifier;
    public float HpModifier;
    public float WaterModifier;

    [Header("Game Event Damage")]
    public float EnergyDamage;
    public float HpDamage;
    public float WaterDamage;

    [Header("Game Event Damage")]
    public float EnergyBonus;
    public float HpBonus;
    public float WaterBonus;

    public GameEvent()
    {
        Random = new System.Random();
        Array values = Enum.GetValues(typeof(GameEventType));
        Type = (GameEventType)values.GetValue(Random.Next(values.Length));

        GenerateGameEvent();
    }

    private void GenerateGameEvent()
    {
        switch (Type)
        {
            case GameEventType.CLIMATE:
                GenerateClimateEvent();
                break;
            case GameEventType.DANGER:
                GenerateDangerEvent();
                break;
            case GameEventType.OTHEREVENT:
                GenerateOtherEvent();
                break;
            default:
                GenerateClimateEvent();
                break;
        }
    }

    private void GenerateClimateEvent()
    {
        Array values = Enum.GetValues(typeof(ClimateEventType));
        ClimateType = (ClimateEventType)values.GetValue(Random.Next(values.Length));
    }

    private void GenerateDangerEvent()
    {
        Array values = Enum.GetValues(typeof(DangerEventType));
        DangerType = (DangerEventType)values.GetValue(Random.Next(values.Length));
    }

    private void GenerateOtherEvent()
    {
        Array values = Enum.GetValues(typeof(OtherEventType));
        OtherType = (OtherEventType)values.GetValue(Random.Next(values.Length));
    }

    public void SetEventModifiers(float energy = 1, float hp = 1, float water = 1)
    {
        this.EnergyModifier = energy;
        this.HpModifier = hp;
        this.WaterModifier = water;
    }

    public void SetEventInstaDamage(float energy = 0, float hp = 0, float water = 0)
    {
        this.EnergyDamage = energy;
        this.HpDamage = hp;
        this.WaterDamage = water;
    }

    public void SetEventInstaBonuses(float energy = 0, float hp = 0, float water = 0)
    {
        this.EnergyBonus = energy;
        this.HpBonus = hp;
        this.WaterBonus = water;
    }
}
