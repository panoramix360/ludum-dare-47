using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkills : MonoBehaviour
{
    [SerializeField] private List<BaseSkill> skills;
    private List<BaseSkill> unlockedSkills;

    public PlayerSkills()
    {
        unlockedSkills = new List<BaseSkill>();
    }

    public void UnlockSkill(BaseSkill skill)
    {
        if (!IsSkillUnlocked(skill))
        {
            unlockedSkills.Add(skill);
        }
    }

    public bool IsSkillUnlocked(BaseSkill skill)
    {
        return unlockedSkills.Contains(skill);
    }
}
