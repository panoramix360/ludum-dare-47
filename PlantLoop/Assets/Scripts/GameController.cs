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
    [SerializeField] private Environment EnvironmentObject;

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
}
