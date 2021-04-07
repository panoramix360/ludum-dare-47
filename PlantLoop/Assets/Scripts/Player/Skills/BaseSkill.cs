using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSkill : ScriptableObject
{
    public Modifier modifier;
    public List<AttributeModifier> costs;
    public BaseSkill requirement;
}
