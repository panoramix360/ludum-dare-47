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

    private bool CheckAndPayAttributesCost(List<AttributeModifier> attrsModifiers)
    {
        bool hasIncome = true;

        float totalStructureToDecrement = 0;
        float totalWaterToDecrement = 0;
        float totalEnergyToDecrement = 0;

        foreach (AttributeModifier attrModifier in attrsModifiers)
        {
            float attributeValueToCompare = 0;
            switch(attrModifier.attr)
            {
                case AttributeEnum.ENERGY:
                    attributeValueToCompare = playerAttributes.energy.value;
                    totalEnergyToDecrement = attrModifier.value;
                    break;
                case AttributeEnum.WATER:
                    attributeValueToCompare = playerAttributes.water.value;
                    totalWaterToDecrement = attrModifier.value;
                    break;
                case AttributeEnum.STRUCTURE:
                    attributeValueToCompare = playerAttributes.structure.value;
                    totalStructureToDecrement = attrModifier.value;
                    break;
            }

            if (attributeValueToCompare < attrModifier.value)
            {
                hasIncome = false;
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
