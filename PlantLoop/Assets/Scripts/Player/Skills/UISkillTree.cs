using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISkillTree : MonoBehaviour
{
    private PlayerSkills playerSkills;

    private void Awake()
    {
        transform.Find("SkillBtn").GetComponent<Button>().onClick.AddListener(OnClickSkill);
    }

    private void OnClickSkill()
    {
        Debug.Log("Skill clicked");
    }

    public void SetPlayerSkills(PlayerSkills playerSkills)
    {
        this.playerSkills = playerSkills;
    }
}
