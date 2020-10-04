using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherEvent : GameEvent
{
    public OtherEvent(GameEvent gameEvent) : base(gameEvent)
    {
        GetEventModifiers();
    }

    private void GetEventModifiers()
    {
        switch (OtherType)
        {
            case OtherEventType.PoorSoilQuality:
                SetEventModifiers(0.7f, -0.1f, 0.5f);
                SetEventInstaDamage(0f, 10f, 10f);
                break;
            default:
                Debug.LogError("Sem tipo de evento Outros");
                break;
        }
    }
}
