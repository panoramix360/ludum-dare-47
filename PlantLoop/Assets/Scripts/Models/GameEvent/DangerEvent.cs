using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerEvent : GameEvent
{
    public DangerEvent(GameEvent gameEvent) : base(gameEvent)
    {
        GetEventModifiers();
    }

    private void GetEventModifiers()
    {
        switch (DangerType)
        {
            case DangerEventType.CATTERPILLAR:
                SetEventModifiers(-0.1f, -0.8f, 0f);
                SetEventInstaDamage(0f, 10f, 0f);
                break;
            case DangerEventType.FUNGUS:
                SetEventModifiers(1f, -0.8f, -0.3f);
                SetEventInstaDamage(0f, 10f, 0f);
                break;
            default:
                Debug.LogError("Sem tipo de evento de perigos");
                break;
        }
    }
}
