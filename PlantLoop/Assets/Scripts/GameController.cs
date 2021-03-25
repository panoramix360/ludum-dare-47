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

    [Header("Game Event Difficulty")]
    [SerializeField] private int difficultyPointsMax = 4;
    private int difficultyPointsLeft = 0;

    [Header("UI")]
    [SerializeField] private GameObject gameEventsContainer;
    [SerializeField] private GameObject climateEventsContainer;
    [SerializeField] private GameObject eventIcon;

    [Header("Player Attributes UI")]
    [SerializeField] private TextMeshProUGUI structureTxt;
    [SerializeField] private TextMeshProUGUI waterTxt;
    [SerializeField] private TextMeshProUGUI energyTxt;
    [SerializeField] private Image structureImg;
    [SerializeField] private Image waterImg;
    [SerializeField] private Image energyImg;
    [SerializeField] private GameObject unitPerTimePrefab;

    [Header("Menu UI")]
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject endGameMenu;
    [SerializeField] private GameObject seedGeneratedPopupPrefab;

    [SerializeField] private GameObject forestPrefab;
    [SerializeField] private GameObject desertPrefab;
    [SerializeField] private GameObject swampPrefab;

    private bool gameIsPaused;

    [Header("Environment Modifiers")]
    private Environment currentEnvironment;
    private List<GameEventV2> gameEventList;

    [SerializeField] private List<Modifier> currentModifiers;

    private float totalStructureModifiers;
    private float totalEnergyModifiers;
    private float totalWaterModifiers;

    private void Awake()
    {
        base.Awake();
        currentModifiers = new List<Modifier>();
        gameEventList = new List<GameEventV2>();

        loading.GetComponent<Animator>().Play("FadeOut");

        SetupLevelUpValuesIfNeeded();
    }

    private void Start()
    {
        //Seta 
        difficultyPointsLeft = difficultyPointsMax;

        InsertModifier(new Modifier(currentEnvironment.Type.ToString(), new List<AttributeModifier>()
        {
            new AttributeModifier(AttributeEnum.STRUCTURE, currentEnvironment.StructureModifier),
            new AttributeModifier(AttributeEnum.ENERGY, currentEnvironment.EnergyModifier),
            new AttributeModifier(AttributeEnum.WATER, currentEnvironment.WaterModifier)
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

    public void UpdatePlayerAttributesUI()
    {
        structureTxt.text = string.Format("{0:0.0}", playerAttributes.structure.value);
        waterTxt.text = string.Format("{0:0.0}", playerAttributes.water.value);
        energyTxt.text = string.Format("{0:0.0}", playerAttributes.energy.value);
        structureImg.fillAmount = playerAttributes.structure.value / playerAttributes.structure.baseValue;
        waterImg.fillAmount = playerAttributes.water.value / playerAttributes.water.baseValue;
        energyImg.fillAmount = playerAttributes.energy.value / playerAttributes.energy.baseValue;
    }

    public void SetupLevelUpValuesIfNeeded()
    {
        if (PersistedObject.Instance.isChanged)
        {
            playerAttributes.structure.baseValue = PersistedObject.Instance.baseStructure;
            playerAttributes.energy.baseValue = PersistedObject.Instance.baseEnergy;
            playerAttributes.water.baseValue = PersistedObject.Instance.baseWater;

            playerAttributes.structure.unitPerTime = PersistedObject.Instance.baseStructureUnitPerTime;
            playerAttributes.energy.unitPerTime = PersistedObject.Instance.baseEnergyUnitPerTime;
            playerAttributes.water.unitPerTime = PersistedObject.Instance.baseWaterUnitPerTime;
        }

        currentEnvironment = new Environment(Enum.GetValues(typeof(EnvironmentType)).Cast<EnvironmentType>().ToList()[PersistedObject.Instance.currentEnvironment]);
        GameObject env = null;
        switch(currentEnvironment.Type)
        {
            case EnvironmentType.FOREST:
                env = Instantiate(forestPrefab);
                break;
            case EnvironmentType.DESERT:
                env = Instantiate(desertPrefab);
                break;
            case EnvironmentType.SWAMP:
                env = Instantiate(swampPrefab);
                break;
        }

        if (env != null)
        {
            Canvas canvas = env.GetComponent<Canvas>();
            canvas.worldCamera = Camera.main;
            canvas.sortingLayerName = "Background";
            foreach(Transform child in env.transform)
            {
                Canvas childCanvas = child.GetComponent<Canvas>();
                if (childCanvas != null)
                {
                    childCanvas.sortingLayerName = "Background";
                }
            }
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
        PersistedObject.Instance.baseStructure = playerAttributes.structure.baseValue;
        PersistedObject.Instance.baseEnergy = playerAttributes.energy.baseValue;
        PersistedObject.Instance.baseWater = playerAttributes.water.baseValue;

        PersistedObject.Instance.baseStructureUnitPerTime = playerAttributes.structure.unitPerTime;
        PersistedObject.Instance.baseEnergyUnitPerTime = playerAttributes.energy.unitPerTime;
        PersistedObject.Instance.baseWaterUnitPerTime = playerAttributes.water.unitPerTime;

        PersistedObject.Instance.currentEnvironment += 1;
        PersistedObject.Instance.currentEnvironment = Math.Min(PersistedObject.Instance.currentEnvironment, 2);

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
        float totalStructure = 0f;
        float totalEnergy = 0f;
        float totalWater = 0f;

        foreach (Modifier modifier in currentModifiers)
        {
            foreach (AttributeModifier entry in modifier.values)
            {
                switch (entry.attr)
                {
                    case AttributeEnum.STRUCTURE:
                        totalStructure += entry.value * playerAttributes.structure.unitPerTime;
                        break;
                    case AttributeEnum.ENERGY:
                        totalEnergy += entry.value * playerAttributes.energy.unitPerTime;
                        break;
                    case AttributeEnum.WATER:
                        totalWater += entry.value * playerAttributes.water.unitPerTime;
                        break;
                    default:
                        Debug.LogError("Atributo não encontrado");
                        break;
                }
            }
        }

        totalStructureModifiers = totalStructure;
        totalEnergyModifiers = totalEnergy;
        totalWaterModifiers = totalWater;
    }

    public void UpdatePlayerAttributesByTimeUnits()
    {
        float structurePerSec = playerAttributes.structure.unitPerTime + totalStructureModifiers;
        float waterPerSec = playerAttributes.water.unitPerTime + totalWaterModifiers;
        float energyPerSec = playerAttributes.energy.unitPerTime + totalEnergyModifiers;
        playerAttributes.structure.IncrementValue(structurePerSec);
        playerAttributes.water.IncrementValue(waterPerSec);
        playerAttributes.energy.IncrementValue(energyPerSec);

        UpdatePlayerAttributesUI();

        CreatePerSecUI(structurePerSec, waterPerSec, energyPerSec);

        CheckIfPlayerLoseHealth(structurePerSec, waterPerSec, energyPerSec);

        if (player.IsDead())
        {
            EndGame();
        }
    }

    private void CheckIfPlayerLoseHealth(float structurePerSec, float waterPerSec, float energyPerSec)
    {
        float amountOfDamage = 0;
        if (playerAttributes.structure.value <= 0)
        {
            amountOfDamage = structurePerSec;
        }
        if (playerAttributes.water.value <= 0)
        {
            amountOfDamage = waterPerSec;
        }
        if (playerAttributes.energy.value <= 0)
        {
            amountOfDamage = energyPerSec;
        }
        player.LoseHealth(amountOfDamage);
    }

    public void CreatePerSecUI(float structurePerSec, float waterPerSec, float energyPerSec)
    {
        Instantiate(unitPerTimePrefab, structureImg.transform).GetComponent<TextMeshProUGUI>().text = string.Format("{0:0.0}", structurePerSec);
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
        if (difficultyPointsLeft > 0)
        {
            Debug.Log("Começou a criar evento");
            //if (gameEventList.Any(x => x.Type == GameEventType.CLIMATE))
            //{
                //Não randomizar eventos de clima caso exista
                //Não fiz isso por enquanto pq sempre vai haver somente 1 evento de clima
            //}
            var gameEvent = new GameEventV2().RandomizeGameEventByDifficultyPoints(difficultyPointsLeft);
            gameEventList.Add(gameEvent);
            difficultyPointsLeft -= gameEvent.DifficultyPoints;

            UpdatePlayerAttributesByEvent(gameEvent);

            AddEventIconUI(gameEvent);

            StartCoroutine(DestroyEventAfter(gameEvent));
        }
    }

    private IEnumerator DestroyEventAfter(GameEventV2 gameEvent)
    {
        yield return new WaitForSeconds(gameEvent.DurationTime);
        RemoveModifier(gameEvent.Identifier);
        RemoveEventIconUI(gameEvent.Identifier);
        gameEventList.Remove(gameEvent);
        difficultyPointsLeft += gameEvent.DifficultyPoints;

    }

    private void AddEventIconUI(GameEventV2 gameEvent)
    {
        if (!string.IsNullOrEmpty(gameEvent.IconPathLeft))
        {
            GameObject iconLeft = Instantiate(eventIcon, climateEventsContainer.transform);
            iconLeft.GetComponent<Image>().sprite = Resources.Load<Sprite>(gameEvent.IconPathLeft);
            iconLeft.name = gameEvent.Identifier;
            iconLeft.GetComponent<EventIcon>().SetEventIconText(gameEvent.Identifier, string.Format("{0:0.0}", gameEvent.WaterModifier), string.Format("{0:0.0}", gameEvent.StructureModifier), string.Format("{0:0.0}", gameEvent.EnergyModifier));
        }
        GameObject icon = Instantiate(eventIcon, gameEventsContainer.transform);
        icon.GetComponent<Image>().sprite = Resources.Load<Sprite>(gameEvent.IconPath);
        icon.name = gameEvent.Identifier;
        icon.GetComponent<EventIcon>().SetEventIconText(gameEvent.Identifier, string.Format("{0:0.0}", gameEvent.WaterModifier), string.Format("{0:0.0}", gameEvent.StructureModifier), string.Format("{0:0.0}", gameEvent.EnergyModifier));
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

        foreach (Transform item in climateEventsContainer.transform)
        {
            if (item.gameObject.name == identifier)
            {
                Destroy(item.gameObject);
            }
        }
    }

    private void UpdatePlayerAttributesByEvent(GameEventV2 gameEvent)
    {
        ApplyInstantBonusEventsInPlayer(gameEvent);
        ApplyInstantDamageEventsInPlayer(gameEvent);
        ApplyEventModifiersInPlayer(gameEvent);
    }

    private void ApplyInstantBonusEventsInPlayer(GameEventV2 gameEvent)
    {
        playerAttributes.structure.IncrementValue(gameEvent.StructureBonus);
        playerAttributes.energy.IncrementValue(gameEvent.EnergyBonus);
        playerAttributes.water.IncrementValue(gameEvent.WaterBonus);
    }

    private void ApplyInstantDamageEventsInPlayer(GameEventV2 gameEvent)
    {
        playerAttributes.structure.DecrementValue(gameEvent.StructureDamage);
        playerAttributes.energy.DecrementValue(gameEvent.EnergyDamage);
        playerAttributes.water.DecrementValue(gameEvent.WaterDamage);
    }

    private void ApplyEventModifiersInPlayer(GameEventV2 gameEvent)
    {
        InsertModifier(new Modifier(gameEvent.Identifier, new List<AttributeModifier>()
        {
            new AttributeModifier(AttributeEnum.STRUCTURE, gameEvent.StructureModifier),
            new AttributeModifier(AttributeEnum.ENERGY, gameEvent.EnergyModifier),
            new AttributeModifier(AttributeEnum.WATER, gameEvent.WaterModifier)
        }));
    }
    #endregion
}
