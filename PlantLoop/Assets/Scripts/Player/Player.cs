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
    [SerializeField] private GameObject leftNode;
    [SerializeField] private GameObject middleNode;
    [SerializeField] private GameObject rightNode;
    
    [Header("Upgrade")]
    [SerializeField] private float upgradeWaterValue;
    [SerializeField] private float upgradeStructureValue;
    [SerializeField] private float upgradeEnergyValue;

    [SerializeField] private float upgradeWaterPerSecValue;
    [SerializeField] private float upgradeStructurePerSecValue;
    [SerializeField] private float upgradeEnergyPerSecValue;

    [Header("SFX")]
    [SerializeField] private AudioClip upgradeSound;

    private int currentLeftUpgradeNode;
    private int currentMiddleUpgradeNode;
    private int currentRightUpgradeNode;

    private bool canUpgradeLeftBranch = true;
    private bool canUpgradeRightBranch = true;

    private PlayerAttributes playerAttributes;
    private PlayerLevelUp playerLevelUp;
    private PlayerHealth playerHealth;
    private PlayerSkills playerSkills;

    private AudioSource audioSource;

    private void Start()
    {
        playerAttributes = GetComponent<PlayerAttributes>();
        playerLevelUp = GetComponent<PlayerLevelUp>();
        playerHealth = GetComponent<PlayerHealth>();
        playerSkills = GetComponent<PlayerSkills>();
        playerSkills.OnSkillUnlocked += PlayerSkills_OnSkillUnlocked;
        FindObjectOfType<UISkillTree>().SetPlayerSkills(playerSkills);
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        int totalUpgradesNode = currentLeftUpgradeNode + currentMiddleUpgradeNode + currentRightUpgradeNode;
        if (totalUpgradesNode >= playerLevelUp.GetUpgradesToLevelUp())
        {
            bool isMaiorityLeft = currentLeftUpgradeNode > currentMiddleUpgradeNode && currentLeftUpgradeNode > currentRightUpgradeNode ? true : false;
            bool isMaiorityMiddle = currentMiddleUpgradeNode > currentLeftUpgradeNode && currentMiddleUpgradeNode > currentRightUpgradeNode ? true : false;
            bool isMaiorityRight = currentRightUpgradeNode > currentLeftUpgradeNode && currentRightUpgradeNode > currentMiddleUpgradeNode ? true : false;

            PlayerLevelUp.LevelType levelType = PlayerLevelUp.LevelType.DRAW;
            if (isMaiorityLeft)
            {
                levelType = PlayerLevelUp.LevelType.WATER;
                playerLevelUp.NextLevel(levelType);
            }
            else if (isMaiorityMiddle)
            {
                levelType = PlayerLevelUp.LevelType.STRUCTURE;
                playerLevelUp.NextLevel(levelType);
            }
            else if (isMaiorityRight)
            {
                levelType = PlayerLevelUp.LevelType.ENERGY;
                playerLevelUp.NextLevel(levelType);
            }
            else
            {
                levelType = PlayerLevelUp.LevelType.DRAW;
                playerLevelUp.NextLevel(levelType);
            }

            currentLeftUpgradeNode = 0;
            currentMiddleUpgradeNode = 0;
            currentRightUpgradeNode = 0;

            GameController.Instance.NextSeed(levelType);
        }
    }

    public bool CheckIfNodeUIShow()
    {
        return nodeUi.activeInHierarchy;
    }

    public void ShowUpgradeNode()
    {
        audioSource.PlayOneShot(upgradeSound);
        nodeUi.SetActive(true);
        leftNode.SetActive(canUpgradeLeftBranch);
        rightNode.SetActive(canUpgradeRightBranch);

        HideAllChilds(leftNode.transform);
        HideAllChilds(middleNode.transform);
        HideAllChilds(rightNode.transform);
    }

    private void HideAllChilds(Transform transform)
    {
        foreach (Transform node in transform)
        {
            node.gameObject.SetActive(false);
        }
    }

    private bool CheckAndPayAttributeCost(Attribute attribute)
    {
        bool hasIncome = true;

        int totalStructureToDecrement = 0;
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
                case AttributeEnum.STRUCTURE:
                    if (playerAttributes.structure.value >= upgradeCost.cost)
                    {
                        totalStructureToDecrement = upgradeCost.cost;
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
            playerAttributes.structure.DecrementValue(totalStructureToDecrement);
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
            playerAttributes.water.IncrementUnitPerTime(upgradeWaterPerSecValue);

            nodeUi.SetActive(false);

            GameController.Instance.UpdatePlayerAttributesUI();

            currentLeftUpgradeNode++;

            GameController.Instance.ResumeGame();
        }
    }

    public void OnClickStructureNode()
    {
        Debug.Log("Middle Node");
        if (CheckAndPayAttributeCost(playerAttributes.structure))
        {
            IncrementMiddleUpgradeBranch();

            playerAttributes.structure.IncrementBaseValue(upgradeStructureValue);
            playerAttributes.water.IncrementBaseValue(upgradeStructureValue);
            playerAttributes.energy.IncrementBaseValue(upgradeStructureValue);

            playerAttributes.structure.IncrementUnitPerTime(upgradeStructurePerSecValue);
            playerAttributes.water.IncrementUnitPerTime(upgradeStructurePerSecValue);
            playerAttributes.energy.IncrementUnitPerTime(upgradeStructurePerSecValue);

            nodeUi.SetActive(false);

            GameController.Instance.UpdatePlayerAttributesUI();

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
            playerAttributes.energy.IncrementUnitPerTime(upgradeEnergyPerSecValue);

            nodeUi.SetActive(false);

            GameController.Instance.UpdatePlayerAttributesUI();

            currentRightUpgradeNode++;

            GameController.Instance.ResumeGame();
        }
    }

    public void OnClickBaseNode()
    {
        Debug.Log("Base Node");
        playerAttributes.structure.ResetValue();
        playerAttributes.energy.ResetValue();
        playerAttributes.water.ResetValue();

        nodeUi.SetActive(false);

        GameController.Instance.UpdatePlayerAttributesUI();

        GameController.Instance.ResumeGame();
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

    public bool IsDead()
    {
        return playerHealth.health <= 0;
    }

    public void LoseHealth(float amount)
    {
        playerHealth.Damage(amount);
    }

    public PlayerSkills GetPlayerSkills()
    {
        return playerSkills;
    }

    private void PlayerSkills_OnSkillUnlocked(object sender, PlayerSkills.OnSkillUnlockedEventArgs e)
    {
        var passiveSkill = e.skill as PassiveSkill;
        if (passiveSkill != null)
        {
            GameController.Instance.InsertModifier(passiveSkill.modifier);
        }
    }
}
