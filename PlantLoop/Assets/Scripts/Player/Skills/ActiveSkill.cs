using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActiveType
{
    PREVENT_WATER_LOSS
}

[CreateAssetMenu(menuName = "Skill/Active")]
public class ActiveSkill : BaseSkill
{
    [Tooltip("Duration in seconds")]
    public float duration;

    [Tooltip("Active type of skill")]
    public ActiveType type;

    [Tooltip("Cooldown in seconds")]
    public float cooldown;
}
