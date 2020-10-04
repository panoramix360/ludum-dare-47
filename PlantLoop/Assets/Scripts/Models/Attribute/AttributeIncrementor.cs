using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributeIncrementor
{
    public AttributeEnum type;
    public float baseIncrementor;
    public float perTimeIncrementor;

    public AttributeIncrementor(AttributeEnum type, float baseIncrementor, float perTimeIncrementor)
    {
        this.type = type;
        this.baseIncrementor = baseIncrementor;
        this.perTimeIncrementor = perTimeIncrementor;
    }
}
