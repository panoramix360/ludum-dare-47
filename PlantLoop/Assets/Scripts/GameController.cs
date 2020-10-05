using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameController : SingletonDestroyable<GameController>
{
    [Header("Game Objects")]
    [SerializeField] private Player player;
    [SerializeField] private PlayerAttributes playerAttributes;
    [SerializeField] private TimeController timeController;
    [SerializeField] private Loading loading;

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
    [SerializeField] private GameObject unitPerTimePrefab;

    [Header("Menu UI")]
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject endGameMenu;
    [SerializeField] private GameObject seedGeneratedPopupPrefab;

    private bool gameIsPaused;

    [Header("Environment Modifiers")]
    [SerializeField] private EnvironmentType initialEnvironmentType;
    private Environment currentEnvironment;
    private List<GameEvent> gameEventList;

    private List<Modifier> currentModifiers;

    private float totalHpModifiers;
    private float totalEnergyModifiers;
    private float totalWaterModifiers;

    private void Awake()
    {
        base.Awake();
        currentModifiers = new List<Modifier>();
        gameEventList = new List<GameEvent>();
        currentEnvironment = new Environment(initialEnvironmentType);

        loading.GetComponent<Animator>().Play("FadeOut");

        SetupLevelUpValuesIfNeeded();
    }

    private void Start()
    {
        InsertModifier(new Modifier(currentEnvironment.Type.ToString(), new Dictionary<AttributeEnum, float>
        {
            [AttributeEnum.HP] = currentEnvironment.HpModifier,
            [AttributeEnum.ENERGY] = currentEnvironment.EnergyModifier,
            [AttributeEnum.WATER] = currentEnvironment.WaterModifier
        }));
    }

    private void Update()
    {
        if (Input.GetKeyDown("escape") && !gameIsPaused)
        {
            gameIsPaused = !gameIsPaused;
            Instantiate(pauseMenu, canvas.transform);
            timeController.PauseTime();
        }
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

    public void SetupLevelUpValuesIfNeeded()
    {
        if (PersistedObject.Instance.isChanged)
        {
            playerAttributes.hp.baseValue = PersistedObject.Instance.baseHp;
            playerAttributes.energy.baseValue = PersistedObject.Instance.baseEnergy;
            playerAttributes.water.baseValue = PersistedObject.Instance.baseWater;

            playerAttributes.hp.unitPerTime = PersistedObject.Instance.baseHpUnitPerTime;
            playerAttributes.energy.unitPerTime = PersistedObject.Instance.baseEnergyUnitPerTime;
            playerAttributes.water.unitPerTime = PersistedObject.Instance.baseWaterUnitPerTime;
        }
    }

    public void UpgradePlayerNode()
    {
        player.ShowUpgradeNode();
        timeController.PauseTime();
    }

    public void UnpauseGame()
    {
        if (!player.CheckIfNodeUIShow())
        {
            timeController.ResumeTime();
        }
        gameIsPaused = false;
    }

    public void NextSeed(PlayerLevelUp.LevelType levelType)
    {
        EndCyclePopup popup = Instantiate(seedGeneratedPopupPrefab, canvas.transform).GetComponent<EndCyclePopup>();
        popup.SetLevelType(levelType);
        popup.SetOnNextCycleOnClick(GoToNextCycle);
        timeController.PauseTime();
    }

    public void GoToNextCycle()
    {
        loading.GetComponent<Animator>().Play("FadeIn");
    }

    public void SetupNewCycle()
    {
        PersistedObject.Instance.isChanged = true;
        PersistedObject.Instance.baseHp = playerAttributes.hp.baseValue;
        PersistedObject.Instance.baseEnergy = playerAttributes.energy.baseValue;
        PersistedObject.Instance.baseWater = playerAttributes.water.baseValue;

        PersistedObject.Instance.baseHpUnitPerTime = playerAttributes.hp.unitPerTime;
        PersistedObject.Instance.baseEnergyUnitPerTime = playerAttributes.energy.unitPerTime;
        PersistedObject.Instance.baseWaterUnitPerTime = playerAttributes.water.unitPerTime;

        SceneManager.LoadScene("MainScene");
        timeController.ResumeTime();
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
        float hpPerSec = playerAttributes.hp.unitPerTime + totalHpModifiers;
        float waterPerSec = playerAttributes.water.unitPerTime + totalWaterModifiers;
        float energyPerSec = playerAttributes.energy.unitPerTime + totalEnergyModifiers;
        playerAttributes.hp.IncrementValue(hpPerSec);
        playerAttributes.water.IncrementValue(waterPerSec);
        playerAttributes.energy.IncrementValue(energyPerSec);

        CreatePerSecUI(hpPerSec, waterPerSec, energyPerSec);

        if (playerAttributes.hp.value <= 0)
        {
            EndGame();
        }

        UpdatePlayerAttributes();
    }

    public void CreatePerSecUI(float hpPerSec, float waterPerSec, float energyPerSec)
    {
        Instantiate(unitPerTimePrefab, hpImg.transform).GetComponent<TextMeshProUGUI>().text = string.Format("{0:0.0}", hpPerSec);
        Instantiate(unitPerTimePrefab, waterImg.transform).GetComponent<TextMeshProUGUI>().text = string.Format("{0:0.0}", waterPerSec);
        Instantiate(unitPerTimePrefab, energyImg.transform).GetComponent<TextMeshProUGUI>().text = string.Format("{0:0.0}", energyPerSec);
    }

    public void EndGame()
    {
        Instantiate(endGameMenu, canvas.transform);
        timeController.PauseTime();
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

        StartCoroutine(DestroyEventAfter(gameEvent));
    }

    private IEnumerator DestroyEventAfter(GameEvent gameEvent)
    {
        yield return new WaitForSeconds(gameEvent.DurationTime);
        RemoveModifier(gameEvent.Identifier);
        RemoveEventIconUI(gameEvent.Identifier);
        gameEventList.Remove(gameEvent);
    }

    private void AddEventIconUI(GameEvent gameEvent)
    {
        if (gameEvent.Type == GameEventType.CLIMATE)
        {
            GameObject iconLeft = Instantiate(eventIcon, climateEventsContainer.transform);
            iconLeft.GetComponent<Image>().sprite = Resources.Load<Sprite>(gameEvent.IconPathLeft);
            iconLeft.name = gameEvent.Identifier;

            GameObject iconRight = Instantiate(eventIcon, gameEventsContainer.transform);
            iconRight.GetComponent<Image>().sprite = Resources.Load<Sprite>(gameEvent.IconPath);
            iconRight.name = gameEvent.Identifier;
        } 
        else
        {
            GameObject icon = Instantiate(eventIcon, gameEventsContainer.transform);
            icon.GetComponent<Image>().sprite = Resources.Load<Sprite>(gameEvent.IconPath);
            icon.name = gameEvent.Identifier;
        }
    }

    private void RemoveEventIconUI(string identifier)
    {
        foreach (Transform item in gameEventsContainer.transform)
        {
            if (item.gameObject.name == identifier)
            {
                Destroy(item.gameObject);
            }
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
