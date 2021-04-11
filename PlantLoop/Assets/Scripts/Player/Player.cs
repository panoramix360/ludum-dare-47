using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerAttributes playerAttributes;
    private PlayerLevelUp playerLevelUp;
    private PlayerHealth playerHealth;
    private PlayerSkills playerSkills;

    private void Start()
    {
        playerAttributes = GetComponent<PlayerAttributes>();
        playerLevelUp = GetComponent<PlayerLevelUp>();
        playerHealth = GetComponent<PlayerHealth>();
        playerSkills = GetComponent<PlayerSkills>();
        playerSkills.OnSkillUnlocked += PlayerSkills_OnSkillUnlocked;
        FindObjectOfType<UISkillTree>().SetPlayerSkills(playerSkills);
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
            playerAttributes.PayAttributesCost(passiveSkill.costs);
            GameController.Instance.InsertModifier(passiveSkill.modifier);
        }
    }
}
