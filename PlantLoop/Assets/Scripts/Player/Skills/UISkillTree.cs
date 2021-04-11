using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISkillTree : MonoBehaviour
{
    private PlayerSkills playerSkills;

    private List<Button> skillButtons;

    private void Awake()
    {
        skillButtons = new List<Button>(transform.GetComponentsInChildren<Button>());

        foreach(Button button in skillButtons)
        {
            AttachOnClickListener(button);
        }
    }

    public void SetPlayerSkills(PlayerSkills playerSkills)
    {
        this.playerSkills = playerSkills;
        playerSkills.OnSkillUnlocked += PlayerSkills_OnSkillUnlocked;
        UpdateVisuals();
    }

    private void AttachOnClickListener(Button buttonToAttach)
    {
        buttonToAttach.onClick.AddListener(() =>
        {
            BaseSkill skill = playerSkills.GetSkillByName(buttonToAttach.name);

            if (playerSkills.IsSkillUnlocked(skill))
            {
                ActiveSkill activeSkill = skill as ActiveSkill;
                if (activeSkill != null && !playerSkills.TryActivateSkill(activeSkill))
                {
                    Debug.Log("Cannot activate " + buttonToAttach.name);
                }
            }
            else if (!playerSkills.TryUnlockSkill(skill))
            {
                Debug.Log("Cannot unlock " + buttonToAttach.name);
            }
        });
    }

    private void UpdateVisuals()
    {
        foreach(Button button in skillButtons)
        {
            BaseSkill skill = playerSkills.GetSkillByName(button.name);
            if (playerSkills.IsSkillUnlocked(skill))
            {
                button.gameObject.GetComponentInChildren<Image>().color = Color.yellow;
            }
            else
            {
                if (playerSkills.CanUnlock(skill))
                {
                    button.gameObject.GetComponentInChildren<Image>().color = Color.green;
                }
                else
                {
                    button.gameObject.GetComponentInChildren<Image>().color = Color.black;
                }
            }
        }
    }

    private void PlayerSkills_OnSkillUnlocked(object sender, PlayerSkills.OnSkillUnlockedEventArgs e)
    {
        UpdateVisuals();
    }
}
