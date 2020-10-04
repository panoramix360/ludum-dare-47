using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : Singleton<GameController>
{
    [Header("Game Objects")]
    [SerializeField] private Player player;
    [SerializeField] private PlayerAttributes playerAttributes;

    [Header("Player Attributes UI")]
    [SerializeField] private TextMeshProUGUI hpTxt;
    [SerializeField] private TextMeshProUGUI waterTxt;
    [SerializeField] private TextMeshProUGUI energyTxt;
    [SerializeField] private Image hpImg;
    [SerializeField] private Image waterImg;
    [SerializeField] private Image energyImg;

    [Header("Environment Modifiers")]
    [SerializeField] private EnvironmentType EnvironmentType;
    private Environment EnvironmentObject;
    private GameEvent GameEvent;

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
    }

    public void UpdatePlayerAttributesByTimeUnits()
    {
        EnvironmentObject = new Environment(EnvironmentType);

        playerAttributes.hp.IncrementValue(playerAttributes.hp.unitPerTime * EnvironmentObject.HpModifier);
        playerAttributes.water.IncrementValue(playerAttributes.water.unitPerTime * EnvironmentObject.WaterModifier);
        playerAttributes.energy.IncrementValue(playerAttributes.energy.unitPerTime * EnvironmentObject.EnergyModifier);

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
        playerAttributes.hp.IncrementValue(GameEvent.HpBonus);
        playerAttributes.energy.IncrementValue(GameEvent.EnergyBonus);
        playerAttributes.water.IncrementValue(GameEvent.WaterBonus);
    }

    private void EventDecrementValue()
    {
        playerAttributes.hp.DecrementValue(GameEvent.HpDamage);
        playerAttributes.energy.DecrementValue(GameEvent.EnergyDamage);
        playerAttributes.water.DecrementValue(GameEvent.WaterDamage);
    }

    private void EventChangeModifierValue()
    {
        playerAttributes.hp.IncrementModifier(GameEvent.HpModifier);
        playerAttributes.energy.IncrementModifier(GameEvent.EnergyModifier);
        playerAttributes.water.IncrementModifier(GameEvent.WaterModifier);
    }
    #endregion
}
