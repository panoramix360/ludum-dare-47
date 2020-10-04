using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class GameController : Singleton<GameController>
{
    [Header("Game Objects")]
    [SerializeField] private Player player;
    [SerializeField] private PlayerAttributes playerAttributes;
    [SerializeField] private TimeController timeController;

    [Header("Player Attributes UI")]
    [SerializeField] private TextMeshProUGUI hpTxt;
    [SerializeField] private TextMeshProUGUI waterTxt;
    [SerializeField] private TextMeshProUGUI energyTxt;
    [SerializeField] private Image hpImg;
    [SerializeField] private Image waterImg;
    [SerializeField] private Image energyImg;

    [Header("Environment Modifiers")]
    [SerializeField] private EnvironmentType initialEnvironmentType;
    private Environment currentEnvironment;
    private GameEvent GameEvent;

    private List<Modifier> currentModifiers;

    private float totalHpModifiers;
    private float totalEnergyModifiers;
    private float totalWaterModifiers;

    private void Start()
    {
        currentModifiers = new List<Modifier>();
        currentEnvironment = new Environment(initialEnvironmentType);
        InsertModifier(new Modifier(currentEnvironment.Type.ToString(), new Dictionary<AttributeEnum, float>
        {
            [AttributeEnum.HP] = currentEnvironment.HpModifier,
            [AttributeEnum.ENERGY] = currentEnvironment.EnergyModifier,
            [AttributeEnum.WATER] = currentEnvironment.WaterModifier
        }));
    }

    public void UpdatePlayerAttributes()
    {
        hpTxt.text = playerAttributes.hp.value.ToString();
        waterTxt.text = playerAttributes.water.value.ToString();
        energyTxt.text = playerAttributes.energy.value.ToString();
        hpImg.fillAmount = playerAttributes.hp.value / playerAttributes.hp.baseValue;
        waterImg.fillAmount = playerAttributes.water.value / playerAttributes.water.baseValue;
        energyImg.fillAmount = playerAttributes.energy.value / playerAttributes.energy.baseValue;
    }

    public void UpgradePlayerNode()
    {
        player.ShowUpgradeNode();
        timeController.PauseTime();
    }

    public void ResumeGame()
    {
        timeController.ResumeTime();
    }

    public void InsertModifier(Modifier modifier)
    {
        currentModifiers.Add(modifier);
        CalculateAndUpdateTotalModifiers();
    }

    public void RemoveModifier(string identifier)
    {
        Modifier modifier = currentModifiers.Find(x => x.identifier == identifier);
        currentModifiers.Remove(modifier);
        CalculateAndUpdateTotalModifiers();
    }

    public void CalculateAndUpdateTotalModifiers()
    {
        float totalHp = 0f;
        float totalEnergy = 0f;
        float totalWater = 0f;

        foreach (Modifier modifier in currentModifiers)
        {
            foreach (KeyValuePair<AttributeEnum, float> entry in modifier.values)
            {
                switch (entry.Key)
                {
                    case AttributeEnum.HP:
                        totalHp += entry.Value * playerAttributes.hp.unitPerTime;
                        break;
                    case AttributeEnum.ENERGY:
                        totalEnergy += entry.Value * playerAttributes.energy.unitPerTime;
                        break;
                    case AttributeEnum.WATER:
                        totalWater += entry.Value * playerAttributes.water.unitPerTime;
                        break;
                    default:
                        Debug.LogError("Atributo não encontrado");
                        break;
                }
            }
        }

        totalHpModifiers = totalHp;
        totalEnergyModifiers = totalEnergy;
        totalWaterModifiers = totalWater;
    }

    public void UpdatePlayerAttributesByTimeUnits()
    {
        playerAttributes.hp.IncrementValue(playerAttributes.hp.unitPerTime + totalHpModifiers);
        playerAttributes.water.IncrementValue(playerAttributes.water.unitPerTime + totalWaterModifiers);
        playerAttributes.energy.IncrementValue(playerAttributes.energy.unitPerTime + totalEnergyModifiers);

        UpdatePlayerAttributes();
    }

    #region EVENT
    public void CreateGameEvent()
    {
        var gameEventBase = new GameEvent();
        switch (gameEventBase.Type)
        {
            case GameEventType.CLIMATE:
                GameEvent = new ClimateEvent(gameEventBase);
                break;
            case GameEventType.DANGER:
                GameEvent = new DangerEvent(gameEventBase);
                break;
            case GameEventType.OTHEREVENT:
                GameEvent = new OtherEvent(gameEventBase);
                break;
            default:
                Debug.LogError("Erro criando evento");
                break;
        }
        UpdatePlayerAttributesByEvent();
    }

    private void UpdatePlayerAttributesByEvent()
    {
        ApplyInstantBonusEventsInPlayer();
        ApplyInstantDamageEventsInPlayer();
        ApplyEventModifiersInPlayer();
    }

    private void ApplyInstantBonusEventsInPlayer()
    {
        playerAttributes.hp.IncrementValue(GameEvent.HpBonus);
        playerAttributes.energy.IncrementValue(GameEvent.EnergyBonus);
        playerAttributes.water.IncrementValue(GameEvent.WaterBonus);
    }

    private void ApplyInstantDamageEventsInPlayer()
    {
        playerAttributes.hp.DecrementValue(GameEvent.HpDamage);
        playerAttributes.energy.DecrementValue(GameEvent.EnergyDamage);
        playerAttributes.water.DecrementValue(GameEvent.WaterDamage);
    }

    private void ApplyEventModifiersInPlayer()
    {
        InsertModifier(new Modifier(GameEvent.Type.ToString(), new Dictionary<AttributeEnum, float>
        {
            [AttributeEnum.HP] = GameEvent.HpModifier,
            [AttributeEnum.ENERGY] = GameEvent.EnergyModifier,
            [AttributeEnum.WATER] = GameEvent.WaterModifier
        }));
    }
    #endregion
}
