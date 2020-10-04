using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimateEvent : GameEvent
{
    public ClimateEvent() : base()
    {
        GetEventModifiers();
    }

    private void GetEventModifiers()
    {
        switch (ClimateType)
        {
            case ClimateEventType.SUNNY:
                SetEventModifiers(2f, 0f, 0.1f);
                break;
            case ClimateEventType.CLOUDY:
                SetEventModifiers(0.5f, 0f, 0f);
                break;
            case ClimateEventType.RAINY:
                SetEventModifiers(0f, 0f, 2f);
                SetEventInstaBonuses(0, 0, 10);
                break;
            default:
                Debug.LogError("Sem tipo de evento de clima");
                break;
        }
    }
}
