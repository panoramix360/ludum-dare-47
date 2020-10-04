using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : Singleton<GameController>
{
    [Header("Game Objects")]
    [SerializeField] private Player player;

    [Header("Player Attributes")]
    [SerializeField] private TextMeshProUGUI hpTxt;
    [SerializeField] private TextMeshProUGUI waterTxt;
    [SerializeField] private TextMeshProUGUI energyTxt;
    [SerializeField] private Image hpImg;
    [SerializeField] private Image waterImg;
    [SerializeField] private Image energyImg;

    [Header("Incrementals based on time")]
    [SerializeField] private float hpPerTime;
    [SerializeField] private float waterPerTime;
    [SerializeField] private float energyPerTime;

    [Header("Environment Modifiers")]
    [SerializeField] private EnvironmentType EnvironmentType;
    private Environment EnvironmentObject;
    private GameEvent GameEvent;

    public void UpdatePlayerAttributes()
    {
        hpTxt.text = player.hp.value.ToString();
        waterTxt.text = player.water.value.ToString();
        energyTxt.text = player.energy.value.ToString();
        hpImg.fillAmount = player.hp.value / player.hp.baseValue;
        waterImg.fillAmount = player.water.value / player.water.baseValue;
        energyImg.fillAmount = player.energy.value / player.energy.baseValue;
    }

    public void UpgradePlayerNode()
    {
        player.ShowUpgradeNode();
    }

    public void UpdatePlayerAttributesByTimeUnits()
    {
        EnvironmentObject = new Environment(EnvironmentType);

        player.IncrementHpAttribute(hpPerTime * EnvironmentObject.HpModifier);
        player.IncrementWaterAttribute(waterPerTime * EnvironmentObject.WaterModifier);
        player.IncrementEnergyAttribute(energyPerTime * EnvironmentObject.EnergyModifier);

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
        EventIncrementValues();
        EventDecrementValue();
        EventChangeModifierValue();
    }

    private void EventIncrementValues()
    {
        player.hp.IncrementValue(GameEvent.HpBonus);
        player.energy.IncrementValue(GameEvent.EnergyBonus);
        player.water.IncrementValue(GameEvent.WaterBonus);
    }

    private void EventDecrementValue()
    {
        player.hp.DecrementValue(GameEvent.HpDamage);
        player.energy.DecrementValue(GameEvent.EnergyDamage);
        player.water.DecrementValue(GameEvent.WaterDamage);
    }

    private void EventChangeModifierValue()
    {
        player.hp.modifier += GameEvent.HpModifier;
        player.energy.modifier += GameEvent.EnergyModifier;
        player.water.modifier += GameEvent.WaterModifier;
    }
    #endregion
}
