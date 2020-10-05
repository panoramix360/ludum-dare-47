using System;
using UnityEngine;

public class GameEvent
{
    public float DurationTime { get; set; }
    public string IconPath { get; set; }
    public string IconPathLeft { get; set; }

    //Game Event Types
    public GameEventType? Type;
    public DangerEventType? DangerType;
    public ClimateEventType? ClimateType;
    public OtherEventType? OtherType;

    //Game Event Modifiers
    public float EnergyModifier;
    public float HpModifier;
    public float WaterModifier;

    //Game Event Damage
    public float EnergyDamage;
    public float HpDamage;
    public float WaterDamage;

    //Game Event Bonus
    public float EnergyBonus;
    public float HpBonus;
    public float WaterBonus;

    public GameEvent(GameEvent gameEvent = null)
    {
        UnityEngine.Random.InitState(System.DateTime.Now.Millisecond);
        //Se o gameevent já foi instanciado e decidido fora, ele só seta as propriedades no filho
        if (gameEvent is null)
        {
            Array values = Enum.GetValues(typeof(GameEventType));
            Type = (GameEventType)values.GetValue(UnityEngine.Random.Range(0, values.Length));

            GenerateGameEvent();
        } 
        else
        {
            UpdateVariables(gameEvent);
        }
    }

    private void UpdateVariables(GameEvent gameEvent)
    {
        this.Type = gameEvent.Type;
        this.DangerType = gameEvent.DangerType;
        this.ClimateType = gameEvent.ClimateType;
        this.OtherType = gameEvent.OtherType;
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
        ClimateType = (ClimateEventType)values.GetValue(UnityEngine.Random.Range(0, values.Length));
    }

    private void GenerateDangerEvent()
    {
        Array values = Enum.GetValues(typeof(DangerEventType));
        DangerType = (DangerEventType)values.GetValue(UnityEngine.Random.Range(0, values.Length));
    }

    private void GenerateOtherEvent()
    {
        Array values = Enum.GetValues(typeof(OtherEventType));
        OtherType = (OtherEventType)values.GetValue(UnityEngine.Random.Range(0, values.Length));
    }

    public void SetEventModifiers(float energy = 1, float hp = 1, float water = 1)
    {
        this.EnergyModifier = energy;
        this.HpModifier = hp;
        this.WaterModifier = water;
    }

    public void SetEventInstaDamage(float energy = 0, float hp = 0, float water = 0)
    {
        //Subtrair a quantidade
        this.EnergyDamage = energy;
        this.HpDamage = hp;
        this.WaterDamage = water;
    }

    public void SetEventInstaBonuses(float energy = 0, float hp = 0, float water = 0)
    {
        //Adicionar a quantidade
        this.EnergyBonus = energy;
        this.HpBonus = hp;
        this.WaterBonus = water;
    }
}
