using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerEvent : GameEvent
{
    public DangerEvent(): base()
    {
        GetEventModifiers();
    }

    private void GetEventModifiers()
    {
        switch (DangerType)
        {
            case DangerEventType.CATTERPILLAR:
                SetEventModifiers(-0.3f, -0.8f, 1f);
                SetEventInstaDamage(0f, -10f, 0f);
                break;
            case DangerEventType.FUNGUS:
                SetEventModifiers(1f, -0.8f, -0.3f);
                SetEventInstaDamage(0f, -10f, 0f);
                break;
            default:
                Debug.LogError("Sem tipo de evento de perigos");
                break;
        }
    }
}
