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
        playerSkills.OnSkillActivated += PlayerSkills_OnSkillActivated;
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
        playerAttributes.PayAttributesCost(e.skill.costs);
        var passiveSkill = e.skill as PassiveSkill;
        if (passiveSkill != null)
        {
            GameController.Instance.InsertModifier(e.skill.modifier);
        }
    }

    private void PlayerSkills_OnSkillActivated(object sender, PlayerSkills.OnSkillActivatedEventArgs e)
    {
        if (e.skill != null)
        {
            Debug.Log("Activate skill!");
            StartCoroutine(OnSkillActivateFinish(e.skill));
            StartCoroutine(OnSkillCooldownFinish(e.skill));
        }
    }

    private IEnumerator OnSkillActivateFinish(ActiveSkill skill)
    {
        GameController.Instance.AddActiveSkill(skill);

        yield return new WaitForSeconds(skill.duration);

        Debug.Log("Deactivating skill!");
        GameController.Instance.RemoveActiveSkill(skill);
    }


    private IEnumerator OnSkillCooldownFinish(ActiveSkill skill)
    {
        yield return new WaitForSeconds(skill.cooldown);

        Debug.Log("Skill cooldown over!");
        playerSkills.RemoveActiveSkillFromCooldown(skill);
    }
}
