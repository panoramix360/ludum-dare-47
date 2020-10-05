using System;
using UnityEngine;
using System.Linq;
using System.Collections;

public class GameEvent : MonoBehaviour
{
    public float DurationTime { get; set; }
    public string IconPath { get; set; }
    public string IconPathLeft { get; set; }
    public bool isDead = false;
    public string Identifier { get; set; }

    //Game Event Types
    public GameEventType? Type;
    public DangerEventType? DangerType;
    public ClimateEventType? ClimateType;
    public OtherEventType? OtherType;
    public BonusEventType? BonusType;

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

    //TODO: REFACTOR
    public GameEvent(GameEvent gameEvent = null, GameEventType? notRandomize = null)
    {
        UnityEngine.Random.InitState(System.DateTime.Now.Millisecond);
        //Se o gameevent já foi instanciado e decidido fora, ele só seta as propriedades no filho
        if (gameEvent is null)
        {
            GenerateGameEvent(notRandomize);
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
        this.BonusType = gameEvent.BonusType;
    }

    //TODO: REFACTOR
    private void GenerateGameEvent(GameEventType? typeNot = null)
    {
        //TODO: REFACTOR
        var values = Enum.GetValues(typeof(GameEventType));
        if (typeNot != null)
        {
            values = values.Cast<GameEventType>().ToList().Where(x => x != typeNot).ToArray();
        }
        Type = (GameEventType)values.GetValue(UnityEngine.Random.Range(0, values.Length));

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
            case GameEventType.BONUS:
                GenerateBonusEvent();
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
        Identifier = ClimateType.ToString();
    }

    private void GenerateDangerEvent()
    {
        Array values = Enum.GetValues(typeof(DangerEventType));
        DangerType = (DangerEventType)values.GetValue(UnityEngine.Random.Range(0, values.Length));
        Identifier = DangerType.ToString();
    }

    private void GenerateOtherEvent()
    {
        Array values = Enum.GetValues(typeof(OtherEventType));
        OtherType = (OtherEventType)values.GetValue(UnityEngine.Random.Range(0, values.Length));
        Identifier = OtherType.ToString();
    }

    private void GenerateBonusEvent()
    {
        Array values = Enum.GetValues(typeof(BonusEventType));
        BonusType = (BonusEventType)values.GetValue(UnityEngine.Random.Range(0, values.Length));
        Identifier = BonusType.ToString();
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

    public void ResetEventModifiers()
    {
        this.EnergyModifier = 0;
        this.HpModifier = 0;
        this.WaterModifier = 0;
    }

    private IEnumerator CountdownTimer()
    {
        yield return new WaitForSeconds(DurationTime);
        ResetEventModifiers();
        isDead = true;
    }

    public void BeginCoroutine()
    {
        StartCoroutine(CountdownTimer());
    }
}
