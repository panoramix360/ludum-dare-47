using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Bases")]
    [SerializeField] private double baseHp;
    [SerializeField] private double baseWater;
    [SerializeField] private double baseEnergy;

    [Header("Player Attributes")]
    [SerializeField] private double initialHp;
    [SerializeField] private double initialWater;
    [SerializeField] private double initialEnergy;

    [SerializeField] public Attribute hp;
    [SerializeField] public Attribute water;
    [SerializeField] public Attribute energy;

    [Header("Node Control")]
    [SerializeField] private GameObject treeBase;
    [SerializeField] private GameObject branchPrefab;

    [SerializeField] private GameObject nodeUi;

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

    public void IncrementHpAttribute(double value)
    {
        hp.IncrementValue(value);
    }

    public void IncrementWaterAttribute(double value)
    {
        water.IncrementValue(value);
    }

    public void IncrementEnergyAttribute(double value)
    {
        energy.IncrementValue(value);
    }

    public void ShowUpgradeNode()
    {
        nodeUi.SetActive(true);
        nodeUi.transform.GetChild(0).gameObject.SetActive(canUpgradeLeftBranch);
        nodeUi.transform.GetChild(2).gameObject.SetActive(canUpgradeRightBranch);
    }

    public void onClickLeftNode()
    {
        Debug.Log("Left Node");
        CreateLeftUpgradeBranch();
        nodeUi.SetActive(false);
    }

    public void onClickMiddleNode()
    {
        Debug.Log("Middle Node");
        IncrementMiddleUpgradeBranch();

        nodeUi.SetActive(false);
    }

    public void onClickRightNode()
    {
        Debug.Log("Right Node");
        CreateRightUpgradeBranch();
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
        treeBase.transform.position = new Vector2(treeBase.transform.position.x, treeBase.transform.position.y + 1);
        canUpgradeLeftBranch = true;
        canUpgradeRightBranch = true;
    }
}
