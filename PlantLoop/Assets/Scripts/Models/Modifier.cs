using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Modifier
{
    public string identifier;
    public Dictionary<AttributeEnum, float> values;

    public Modifier(string identifier, Dictionary<AttributeEnum, float> values)
    {
        this.identifier = identifier;
        this.values = values;
    }
}
