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

    [Header("UI")]
    [SerializeField] private GameObject gameEventsContainer;
    [SerializeField] private GameObject climateEventsContainer;
    [SerializeField] private GameObject eventIcon;

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
    private List<GameEvent> gameEventList;

    private List<Modifier> currentModifiers;

    private float totalHpModifiers;
    private float totalEnergyModifiers;
    private float totalWaterModifiers;

    private void Start()
    {
        currentModifiers = new List<Modifier>();
        gameEventList = new List<GameEvent>();
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
        Debug.Log("Criou evento");
        //TODO:REFACTOR
        GameEventType? notRandomize = null;
        if (gameEventList.Any(x => x.Type == GameEventType.CLIMATE))
        {
            notRandomize = GameEventType.CLIMATE;
        }
        var gameEvent = new GameEvent(null, notRandomize);
        switch (gameEvent.Type)
        {
            case GameEventType.CLIMATE:
                gameEvent = new ClimateEvent(gameEvent);
                gameEventList.Add(gameEvent);
                break;
            case GameEventType.DANGER:
                gameEvent = new DangerEvent(gameEvent);
                gameEventList.Add(gameEvent);
                break;
            case GameEventType.OTHEREVENT:
                gameEvent = new OtherEvent(gameEvent);
                gameEventList.Add(gameEvent);
                break;
            case GameEventType.BONUS:
                gameEvent = new BonusEvent(gameEvent);
                gameEventList.Add(gameEvent);
                break;
            default:
                Debug.LogError("Erro criando evento");
                break;
        }
        
        UpdatePlayerAttributesByEvent(gameEvent);

        AddEventIconUI(gameEvent);
    }

    private void AddEventIconUI(GameEvent gameEvent)
    {
        if (gameEvent.Type == GameEventType.CLIMATE)
        {
            GameObject iconLeft = Instantiate(eventIcon, climateEventsContainer.transform);
            iconLeft.GetComponent<Image>().sprite = Resources.Load<Sprite>(gameEvent.IconPathLeft);

            GameObject iconRight = Instantiate(eventIcon, gameEventsContainer.transform);
            iconRight.GetComponent<Image>().sprite = Resources.Load<Sprite>(gameEvent.IconPath);
        } 
        else
        {
            GameObject icon = Instantiate(eventIcon, gameEventsContainer.transform);
            icon.GetComponent<Image>().sprite = Resources.Load<Sprite>(gameEvent.IconPath);
        }
    }

    private void UpdatePlayerAttributesByEvent(GameEvent gameEvent)
    {
        ApplyInstantBonusEventsInPlayer(gameEvent);
        ApplyInstantDamageEventsInPlayer(gameEvent);
        ApplyEventModifiersInPlayer(gameEvent);
    }

    private void ApplyInstantBonusEventsInPlayer(GameEvent gameEvent)
    {
        playerAttributes.hp.IncrementValue(gameEvent.HpBonus);
        playerAttributes.energy.IncrementValue(gameEvent.EnergyBonus);
        playerAttributes.water.IncrementValue(gameEvent.WaterBonus);
    }

    private void ApplyInstantDamageEventsInPlayer(GameEvent gameEvent)
    {
        playerAttributes.hp.DecrementValue(gameEvent.HpDamage);
        playerAttributes.energy.DecrementValue(gameEvent.EnergyDamage);
        playerAttributes.water.DecrementValue(gameEvent.WaterDamage);
    }

    private void ApplyEventModifiersInPlayer(GameEvent gameEvent)
    {
        InsertModifier(new Modifier(gameEvent.Type.ToString(), new Dictionary<AttributeEnum, float>
        {
            [AttributeEnum.HP] = gameEvent.HpModifier,
            [AttributeEnum.ENERGY] = gameEvent.EnergyModifier,
            [AttributeEnum.WATER] = gameEvent.WaterModifier
        }));
    }
    #endregion
}
