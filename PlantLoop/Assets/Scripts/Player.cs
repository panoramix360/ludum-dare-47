using System;
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

    private bool canUpgradeLeftBranch = true;
    private bool canUpgradeRightBranch = true;

    private void Awake()
    {
        SetupPlayerAttributes();
    }

    private void Start()
    {
        GameController.Instance.UpdatePlayerAttributes();
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

    public void onClickLeftNode()
    {
        Debug.Log("Left Node");
        CreateLeftUpgradeBranch();
        water.UpgradeBaseValue(upgradeLeftValue);
        nodeUi.SetActive(false);
    }

    public void onClickMiddleNode()
    {
        Debug.Log("Middle Node");
        IncrementMiddleUpgradeBranch();
        hp.UpgradeBaseValue(upgradeMiddleValue);
        water.UpgradeBaseValue(upgradeMiddleValue);
        energy.UpgradeBaseValue(upgradeMiddleValue);
        nodeUi.SetActive(false);
    }

    public void onClickRightNode()
    {
        Debug.Log("Right Node");
        CreateRightUpgradeBranch();
        energy.UpgradeBaseValue(upgradeRightValue);
        nodeUi.SetActive(false);
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
