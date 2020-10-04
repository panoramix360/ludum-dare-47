﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Bases")]
    [SerializeField] private float baseHp;
    [SerializeField] private float baseWater;
    [SerializeField] private float baseEnergy;

    [Header("Player Attributes")]
    [SerializeField] private float initialHp;
    [SerializeField] private float initialWater;
    [SerializeField] private float initialEnergy;

    [SerializeField] public Attribute hp;
    [SerializeField] public Attribute water;
    [SerializeField] public Attribute energy;

    [Header("Node Control")]
    [SerializeField] private GameObject treeBase;
    [SerializeField] private GameObject branchPrefab;
    [SerializeField] private GameObject nodeUi;
    [SerializeField] private int middleUpgradeIncrementUnit;
    [SerializeField] private float upgradeLeftValue;
    [SerializeField] private float upgradeMiddleValue;
    [SerializeField] private float upgradeRightValue;

    private int currentLeftUpgradeNode;
    private int currentMiddleUpgradeNode;
    private int currentRightUpgradeNode;

    private bool canUpgradeLeftBranch = true;
    private bool canUpgradeRightBranch = true;

    private PlayerLevelUp playerLevelUp;

    private void Awake()
    {
        SetupPlayerAttributes();
    }

    private void Start()
    {
        GameController.Instance.UpdatePlayerAttributes();

        playerLevelUp = GetComponent<PlayerLevelUp>();
    }

    private void Update()
    {
        int totalUpgradesNode = currentLeftUpgradeNode + currentMiddleUpgradeNode + currentRightUpgradeNode;
        if (totalUpgradesNode >= playerLevelUp.GetUpgradesToLevelUp())
        {
            bool isMaiorityLeft = currentLeftUpgradeNode > currentMiddleUpgradeNode && currentLeftUpgradeNode > currentRightUpgradeNode ? true : false;
            bool isMaiorityMiddle = currentMiddleUpgradeNode > currentLeftUpgradeNode && currentMiddleUpgradeNode > currentRightUpgradeNode ? true : false;
            bool isMaiorityRight = currentRightUpgradeNode > currentLeftUpgradeNode && currentRightUpgradeNode > currentMiddleUpgradeNode ? true : false;
            if (isMaiorityLeft)
            {
                playerLevelUp.NextLevel(PlayerLevelUp.LevelType.WATER);
            }
            else if (isMaiorityMiddle)
            {
                playerLevelUp.NextLevel(PlayerLevelUp.LevelType.HP);
            }
            else if (isMaiorityRight)
            {
                playerLevelUp.NextLevel(PlayerLevelUp.LevelType.ENERGY);
            }
            else
            {
                playerLevelUp.NextLevel(PlayerLevelUp.LevelType.DRAW);
            }

            currentLeftUpgradeNode = 0;
            currentMiddleUpgradeNode = 0;
            currentRightUpgradeNode = 0;
        }
    }

    private void SetupPlayerAttributes()
    {
        hp = new HpAttribute(baseHp, initialHp);
        water = new WaterAttribute(baseWater, initialWater);
        energy = new EnergyAttribute(baseEnergy, initialEnergy);
    }

    public void IncrementHpAttribute(float value)
    {
        hp.IncrementValue(value);
    }

    public void IncrementWaterAttribute(float value)
    {
        water.IncrementValue(value);
    }

    public void IncrementEnergyAttribute(float value)
    {
        energy.IncrementValue(value);
    }

    public void ShowUpgradeNode()
    {
        nodeUi.SetActive(true);
        nodeUi.transform.GetChild(0).gameObject.SetActive(canUpgradeLeftBranch);
        nodeUi.transform.GetChild(2).gameObject.SetActive(canUpgradeRightBranch);

        HideAllChilds(nodeUi.transform.GetChild(0));
        HideAllChilds(nodeUi.transform.GetChild(1));
        HideAllChilds(nodeUi.transform.GetChild(2));
    }
    
    private void HideAllChilds(Transform transform)
    {
        foreach(Transform node in transform)
        {
            node.gameObject.SetActive(false);
        }
    }

    private bool CheckAndPayAttributeCost(Attribute attribute)
    {
        bool hasIncome = true;

        int totalHpToDecrement = 0;
        int totalWaterToDecrement = 0;
        int totalEnergyToDecrement = 0;

        foreach (UpgradeCost upgradeCost in attribute.upgradeCosts)
        {
            switch(upgradeCost.attributeType)
            {
                case AttributeEnum.ENERGY:
                    if (energy.value >= upgradeCost.cost)
                    {
                        totalEnergyToDecrement = upgradeCost.cost;
                    }
                    else
                    {
                        hasIncome = false;
                    }
                    break;
                case AttributeEnum.WATER:
                    if (water.value >= upgradeCost.cost)
                    {
                        totalWaterToDecrement = upgradeCost.cost;
                    }
                    else
                    {
                        hasIncome = false;
                    }
                    break;
                case AttributeEnum.HP:
                    if (hp.value >= upgradeCost.cost)
                    {
                        totalHpToDecrement = upgradeCost.cost;
                    }
                    else
                    {
                        hasIncome = false;
                    }
                    break;
            }
        }

        if (hasIncome)
        {
            hp.DecrementValue(totalHpToDecrement);
            water.DecrementValue(totalWaterToDecrement);
            energy.DecrementValue(totalEnergyToDecrement);
        }

        return hasIncome;
    }

    public void onClickLeftNode()
    {
        Debug.Log("Left Node");

        if (CheckAndPayAttributeCost(water))
        {
            CreateLeftUpgradeBranch();

            water.UpgradeBaseValue(upgradeLeftValue);

            nodeUi.SetActive(false);

            GameController.Instance.UpdatePlayerAttributes();

            currentLeftUpgradeNode++;
        }
    }

    public void onClickMiddleNode()
    {
        Debug.Log("Middle Node");
        if (CheckAndPayAttributeCost(hp))
        {
            IncrementMiddleUpgradeBranch();

            hp.UpgradeBaseValue(upgradeMiddleValue);

            water.UpgradeBaseValue(upgradeMiddleValue);

            energy.UpgradeBaseValue(upgradeMiddleValue);

            nodeUi.SetActive(false);

            GameController.Instance.UpdatePlayerAttributes();

            currentMiddleUpgradeNode++;
        }
    }

    public void onClickRightNode()
    {
        Debug.Log("Right Node");
        if (CheckAndPayAttributeCost(energy))
        {
            CreateRightUpgradeBranch();

            energy.UpgradeBaseValue(upgradeRightValue);

            nodeUi.SetActive(false);

            GameController.Instance.UpdatePlayerAttributes();

            currentRightUpgradeNode++;
        }
    }

    public void CreateLeftUpgradeBranch()
    {
        if (!canUpgradeLeftBranch) return;

        Instantiate(branchPrefab, treeBase.transform.position, Quaternion.identity, transform);

        canUpgradeLeftBranch = false;
    }

    public void CreateRightUpgradeBranch()
    {
        if (!canUpgradeRightBranch) return;

        GameObject branch = Instantiate(branchPrefab, treeBase.transform.position, Quaternion.identity, transform);
        branch.GetComponent<SpriteRenderer>().flipX = true;

        canUpgradeRightBranch = false;
    }

    public void IncrementMiddleUpgradeBranch()
    {
        treeBase.transform.position = new Vector2(treeBase.transform.position.x, treeBase.transform.position.y + middleUpgradeIncrementUnit);
        canUpgradeLeftBranch = true;
        canUpgradeRightBranch = true;
    }
}
