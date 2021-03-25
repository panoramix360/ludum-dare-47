using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Modifier
{
    public string identifier;
    public List<AttributeModifier> values;

    public Modifier(string identifier, List<AttributeModifier> values)
    {
        this.identifier = identifier;
        this.values = values;
    }
}
