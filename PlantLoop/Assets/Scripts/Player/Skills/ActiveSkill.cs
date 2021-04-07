using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skill/Active")]
public class ActiveSkill : BaseSkill
{
    [Tooltip("Duration in seconds")]
    public float duration;
    [Tooltip("Cooldown in seconds")]
    public float cooldown;
}
