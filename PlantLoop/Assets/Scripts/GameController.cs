using System;
using TMPro;
using UnityEngine;

public class GameController : Singleton<GameController>
{
    [Header("Game Objects")]
    [SerializeField] private Player player;

    [Header("Player Attributes")]
    [SerializeField] private TextMeshProUGUI hpTxt;
    [SerializeField] private TextMeshProUGUI waterTxt;
    [SerializeField] private TextMeshProUGUI energyTxt;

    [Header("Incrementals based on time")]
    [SerializeField] private double hpPerTime;
    [SerializeField] private double waterPerTime;
    [SerializeField] private double energyPerTime;

    public void UpdatePlayerAttributes()
    {
        hpTxt.text = player.hp.value.ToString();
        waterTxt.text = player.water.value.ToString();
        energyTxt.text = player.energy.value.ToString();
    }

    public void UpdatePlayerAttributesByTimeUnits()
    {
        player.IncrementHpAttribute(hpPerTime);
        player.IncrementWaterAttribute(waterPerTime);
        player.IncrementEnergyAttribute(energyPerTime);

        UpdatePlayerAttributes();
    }
}
