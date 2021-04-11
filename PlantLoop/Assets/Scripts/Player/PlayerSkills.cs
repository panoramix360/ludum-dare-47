using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkills : MonoBehaviour
{
    public event EventHandler<OnSkillUnlockedEventArgs> OnSkillUnlocked;
    public class OnSkillUnlockedEventArgs : EventArgs
    {
        public BaseSkill skill;
    }

    public event EventHandler<OnSkillActivatedEventArgs> OnSkillActivated;
    public class OnSkillActivatedEventArgs : EventArgs
    {
        public ActiveSkill skill;
    }

    [SerializeField] private List<BaseSkill> skills;
    private List<BaseSkill> unlockedSkills;
    private List<ActiveSkill> activeSkillsCooldown;
    private PlayerAttributes playerAttributes;

    private void Awake()
    {
        playerAttributes = GetComponent<PlayerAttributes>();
    }

    public PlayerSkills()
    {
        unlockedSkills = new List<BaseSkill>();
        activeSkillsCooldown = new List<ActiveSkill>();
    }

    private void UnlockSkill(BaseSkill skill)
    {
        if (!IsSkillUnlocked(skill))
        {
            unlockedSkills.Add(skill);
            OnSkillUnlocked?.Invoke(this, new OnSkillUnlockedEventArgs { skill = skill });
        }
    }

    private void ActivateSkill(ActiveSkill skill)
    {
        if (!IsSkillActive(skill))
        {
            activeSkillsCooldown.Add(skill);
            OnSkillActivated?.Invoke(this, new OnSkillActivatedEventArgs { skill = skill });
        }
    }

    public bool TryUnlockSkill(BaseSkill skill)
    {
        if (CanUnlock(skill) && playerAttributes.CanPayAttributes(skill.costs))
        {
            UnlockSkill(skill);
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool TryActivateSkill(ActiveSkill skill)
    {
        if (!IsSkillActive(skill))
        {
            ActivateSkill(skill);
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsSkillUnlocked(BaseSkill skill)
    {
        return unlockedSkills.Contains(skill);
    }

    public bool IsSkillActive(ActiveSkill skill)
    {
        return activeSkillsCooldown.Contains(skill);
    }

    public bool CanUnlock(BaseSkill skill)
    {
        BaseSkill requirement = GetSkillRequirement(skill);

        if (requirement != null)
        {
            if (IsSkillUnlocked(requirement))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        return true;
    }

    public void RemoveActiveSkillFromCooldown(ActiveSkill skill)
    {
        activeSkillsCooldown.Remove(skill);
    }

    public BaseSkill GetSkillByName(string name)
    {
        foreach(BaseSkill skill in skills)
        {
            if (skill.name == name)
            {
                return skill;
            }
        }

        throw new Exception("Skill non existent");
    }

    public BaseSkill GetSkillRequirement(BaseSkill baseSkill)
    {
        return baseSkill.requirement;
    }
}
