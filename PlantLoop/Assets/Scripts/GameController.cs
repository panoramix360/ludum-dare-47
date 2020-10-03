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
    [SerializeField] private double hpPerTime;
    [SerializeField] private double waterPerTime;
    [SerializeField] private double energyPerTime;

    [Header("Environment Modifiers")]
    [SerializeField] private EnvironmentType EnvironmentType;
    [SerializeField] private Environment EnvironmentObject;

    public void UpdatePlayerAttributes()
    {
        hpTxt.text = player.hp.value.ToString();
        waterTxt.text = player.water.value.ToString();
        energyTxt.text = player.energy.value.ToString();
        hpImg.fillAmount = (float)(player.hp.value / player.hp.baseValue);
        waterImg.fillAmount = (float)(player.water.value / player.hp.baseValue);
        energyImg.fillAmount = (float)(player.energy.value / player.hp.baseValue);
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
