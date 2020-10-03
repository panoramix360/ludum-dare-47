using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Attribute
{
    [SerializeField] private AttributeEnum type;
    [SerializeField] private double value;
    [SerializeField] private double modifier;
}
