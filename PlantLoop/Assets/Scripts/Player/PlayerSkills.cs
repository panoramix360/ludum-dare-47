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

    [SerializeField] private List<BaseSkill> skills;
    private List<BaseSkill> unlockedSkills;

    public PlayerSkills()
    {
        unlockedSkills = new List<BaseSkill>();
    }

    private void UnlockSkill(BaseSkill skill)
    {
        if (!IsSkillUnlocked(skill))
        {
            unlockedSkills.Add(skill);
            OnSkillUnlocked?.Invoke(this, new OnSkillUnlockedEventArgs { skill = skill });
        }
    }

    public bool TryUnlockSkill(BaseSkill skill)
    {
        if (CanUnlock(skill))
        {
            UnlockSkill(skill);
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
