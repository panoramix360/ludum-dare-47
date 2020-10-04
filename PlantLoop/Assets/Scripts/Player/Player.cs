using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Node Control")]
    [SerializeField] private GameObject treeBase;
    [SerializeField] private GameObject branchPrefab;
    [SerializeField] private GameObject nodeUi;
    [SerializeField] private float upgradeWaterValue;
    [SerializeField] private float upgradeHpValue;
    [SerializeField] private float upgradeEnergyValue;

    private int currentLeftUpgradeNode;
    private int currentMiddleUpgradeNode;
    private int currentRightUpgradeNode;

    private bool canUpgradeLeftBranch = true;
    private bool canUpgradeRightBranch = true;

    private PlayerAttributes playerAttributes;
    private PlayerLevelUp playerLevelUp;

    private void Start()
    {
        playerAttributes = GetComponent<PlayerAttributes>();
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
                    if (playerAttributes.energy.value >= upgradeCost.cost)
                    {
                        totalEnergyToDecrement = upgradeCost.cost;
                    }
                    else
                    {
                        hasIncome = false;
                    }
                    break;
                case AttributeEnum.WATER:
                    if (playerAttributes.water.value >= upgradeCost.cost)
                    {
                        totalWaterToDecrement = upgradeCost.cost;
                    }
                    else
                    {
                        hasIncome = false;
                    }
                    break;
                case AttributeEnum.HP:
                    if (playerAttributes.hp.value >= upgradeCost.cost)
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
            playerAttributes.hp.DecrementValue(totalHpToDecrement);
            playerAttributes.water.DecrementValue(totalWaterToDecrement);
            playerAttributes.energy.DecrementValue(totalEnergyToDecrement);
        }

        return hasIncome;
    }

    public void OnClickWaterNode()
    {
        Debug.Log("Left Node");

        if (CheckAndPayAttributeCost(playerAttributes.water))
        {
            CreateLeftUpgradeBranch();

            playerAttributes.water.IncrementBaseValue(upgradeWaterValue);

            nodeUi.SetActive(false);

            GameController.Instance.UpdatePlayerAttributes();

            currentLeftUpgradeNode++;

            GameController.Instance.ResumeGame();
        }
    }

    public void OnClickHpNode()
    {
        Debug.Log("Middle Node");
        if (CheckAndPayAttributeCost(playerAttributes.hp))
        {
            IncrementMiddleUpgradeBranch();

            playerAttributes.hp.IncrementBaseValue(upgradeHpValue);

            playerAttributes.water.IncrementBaseValue(upgradeHpValue);

            playerAttributes.energy.IncrementBaseValue(upgradeHpValue);

            nodeUi.SetActive(false);

            GameController.Instance.UpdatePlayerAttributes();

            currentMiddleUpgradeNode++;

            GameController.Instance.ResumeGame();
        }
    }

    public void OnClickEnergyNode()
    {
        Debug.Log("Right Node");
        if (CheckAndPayAttributeCost(playerAttributes.energy))
        {
            CreateRightUpgradeBranch();

            playerAttributes.energy.IncrementBaseValue(upgradeEnergyValue);

            nodeUi.SetActive(false);

            GameController.Instance.UpdatePlayerAttributes();

            currentRightUpgradeNode++;

            GameController.Instance.ResumeGame();
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
        treeBase.transform.position = new Vector2(treeBase.transform.position.x, treeBase.transform.position.y + 2);
        canUpgradeLeftBranch = true;
        canUpgradeRightBranch = true;
    }
}
