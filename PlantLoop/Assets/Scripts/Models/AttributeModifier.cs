using System;

[Serializable]
public class AttributeModifier
{
    public AttributeEnum attr;
    public float value;

    public AttributeModifier(AttributeEnum attr, float value)
    {
        this.attr = attr;
        this.value = value;
    }
}
