using Packages.Rider.Editor.UnitTesting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public struct GameEventV2
{
    public float DurationTime { get; set; }
    public string Identifier { get; set; }
    public string IconPath { get; set; }
    public string IconPathLeft { get; set; }
    public int DifficultyPoints;

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

    public GameEventV2(int difficultyPoints,
        float durationTime,
        string identifier,
        string iconPath,
        string iconPathLeft,
        float energyModifier = 0, 
        float hpModifier = 0, 
        float waterModifier = 0, 
        float energyDamage = 0, 
        float hpDamage = 0, 
        float waterDamage = 0, 
        float energyBonus = 0, 
        float hpBonus = 0, 
        float waterBonus = 0)
    {
        DifficultyPoints = difficultyPoints;
        DurationTime = durationTime;
        Identifier = identifier;
        IconPath = iconPath;
        IconPathLeft = iconPathLeft;
        EnergyModifier = energyModifier;
        HpModifier = hpModifier;
        WaterModifier = waterModifier;
        EnergyDamage = energyDamage;
        HpDamage = hpDamage;
        WaterDamage = waterDamage;
        EnergyBonus = energyBonus;
        HpBonus = hpBonus;
        WaterBonus = waterBonus;
    }

    // DurationTime, Identifier, IconPath, IconPathLeft, EnergyModifier, HpModifier, WaterModifier, EnergyDamage, HpDamage, WaterDamage, EnergyBonus, HpBonus, WaterBonus

    #region CLIMATE
    public static readonly GameEventV2 SUNNY = new GameEventV2(4, 60, "Sunny", "event_ensolarado", "clima_ensolarado", 1f, -0.2f, -0.5f);
    public static readonly GameEventV2 CLOUDY = new GameEventV2(4, 60, "Cloudy", "event_nublado", "clima_nublado", -0.5f, 0f, 0f);
    public static readonly GameEventV2 RAINY = new GameEventV2(4, 60, "Rainy", "event_chuva", "clima_chuva", -1f, 0f, 1f, 0f, 0f, 0f, 0f, 0f, 10f);
    public static readonly GameEventV2 STORMY = new GameEventV2(4, 60, "Stormy", "event_tempestade", "clima_tempestade", -1f, -0.5f, 1f, 0f, 20f, 0f, 0f, 0f, 20f);
    public static readonly GameEventV2 HURRICANY = new GameEventV2(4, 60, "Hurricany", "event_furacao", "clima_furacao", -0.2f, -0.7f, 0f, 0f, 20f, 0f);
    public static readonly GameEventV2 WINDY = new GameEventV2(4, 60, "Windy", "event_ventania", "clima_ventania", -0.5f, -0.3f, 0f);
    #endregion

    #region BONUS
    public static readonly GameEventV2 BEES = new GameEventV2(2, 10, "Bees", "bonus_abelha", null, 0f, 1f, 0f, 0f, 3f, 0f);
    public static readonly GameEventV2 LADYBUG = new GameEventV2(2, 10, "LadyBug", "bonus_joaninha", null, 0.5f, 1f, 0f, 0f, 10f, 0f);
    public static readonly GameEventV2 WORM = new GameEventV2(2, 10, "Worm", "bonus_minhoca", null, 1f, 0f, 1f, 10f, 0f, 10f);
    #endregion

    #region OTHERS
    public static readonly GameEventV2 POORSOILQUALITY = new GameEventV2(3, 30, "Poor Soil Quality", "event_solo_desgastado", null, -1f, -0.3f, -1f, 0f, 10f, 10f);
    public static readonly GameEventV2 GOODSOILQUALITY = new GameEventV2(3, 30, "Good Soil Quality", "event_solo_fertil", null, 0.5f, 0.5f, 0.5f, 5f, 5f, 5f);
    #endregion

    #region DANGER
    public static readonly GameEventV2 CATTERPILLAR = new GameEventV2(1, 10, "Catterpillar", "danger_lagarta", null, -0.1f, -1f, 0f, 0f, 10f, 0f);
    public static readonly GameEventV2 FUNGUS = new GameEventV2(1, 10, "Fungus", "danger_fungos", null, -0.5f, -1f, -0.5f, 0f, 10f, 0f);
    public static readonly GameEventV2 FIRE = new GameEventV2(1, 15, "Fire", "danger_queimada", null, 0f, -2f, -0.8f, 0f, 10f, 0f);
    public static readonly GameEventV2 GRASSHOPPER = new GameEventV2(1, 10, "Grasshopper", "danger_gafanhoto", null, -0.6f, -1f, -0.3f, 0f, 10f, 0f);
    public static readonly GameEventV2 WEED = new GameEventV2(1, 15, "Weed", "danger_erva_daninha", null, -0.2f, -0.2f, -0.5f, 0f, 5f, 0f);
    #endregion

    public List<GameEventV2> ListAllGameEvents()
    {
        var gameEventsList = new List<GameEventV2>();
        gameEventsList.Add(GameEventV2.SUNNY);
        gameEventsList.Add(GameEventV2.CLOUDY);
        gameEventsList.Add(GameEventV2.RAINY);
        gameEventsList.Add(GameEventV2.STORMY);
        gameEventsList.Add(GameEventV2.HURRICANY);
        gameEventsList.Add(GameEventV2.WINDY);

        gameEventsList.Add(GameEventV2.BEES);
        gameEventsList.Add(GameEventV2.LADYBUG);
        gameEventsList.Add(GameEventV2.WORM);

        gameEventsList.Add(GameEventV2.POORSOILQUALITY);
        gameEventsList.Add(GameEventV2.GOODSOILQUALITY);

        gameEventsList.Add(GameEventV2.CATTERPILLAR);
        gameEventsList.Add(GameEventV2.FUNGUS);
        gameEventsList.Add(GameEventV2.FIRE);
        gameEventsList.Add(GameEventV2.GRASSHOPPER);
        gameEventsList.Add(GameEventV2.WEED);

        return gameEventsList;
    }

    public List<GameEventV2> ListGameEventByDifficultyPoints(int difficultyPointsQuant)
    {
        return ListAllGameEvents().Where(x => x.DifficultyPoints <= difficultyPointsQuant).ToList();
    }

    public GameEventV2 RandomizeGameEventByDifficultyPoints(int difficultyPointsQuant)
    {
        var list = ListGameEventByDifficultyPoints(difficultyPointsQuant);
        return list[UnityEngine.Random.Range(1, list.Count)];
    }

}
