using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimateEvent : GameEvent
{
    public ClimateEvent(GameEvent gameEvent) : base(gameEvent)
    {
        GetEventModifiers();
    }

    private void GetEventModifiers()
    {
        switch (ClimateType)
        {
            case ClimateEventType.SUNNY:
                SetEventModifiers(1f, 0f, -0.5f);
                DurationTime = 60;
                IconPath = "event_ensolarado";
                IconPathLeft = "clima_ensolarado";
                break;
            case ClimateEventType.CLOUDY:
                SetEventModifiers(-0.5f, 0f, 0f);
                DurationTime = 60;
                IconPath = "event_nublado";
                IconPathLeft = "clima_nublado";
                break;
            case ClimateEventType.RAINY:
                SetEventModifiers(-1f, 0f, 1f);
                SetEventInstaBonuses(0f, 0f, 10f);
                DurationTime = 60;
                IconPath = "event_chuva";
                IconPathLeft = "clima_chuva";
                break;
            case ClimateEventType.STORMY:
                SetEventModifiers(-1f, -0.2f, 1f);
                SetEventInstaBonuses(0f, 0f, 20f);
                SetEventInstaDamage(0f, 20f, 0f);
                DurationTime = 60;
                IconPath = "event_tempestade";
                IconPathLeft = "clima_tempestade";
                break;
            case ClimateEventType.HURRICANY:
                SetEventModifiers(-0.2f, -0.5f, 0f);
                SetEventInstaDamage(0f, 20f, 0f);
                DurationTime = 60;
                IconPath = "event_furacao";
                IconPathLeft = "clima_furacao";
                break;
            case ClimateEventType.WINDY:
                SetEventModifiers(-0.5f, -0.3f, 0f);
                DurationTime = 60;
                IconPath = "event_ventania";
                IconPathLeft = "clima_ventania";
                break;
            default:
                Debug.LogError("Sem tipo de evento de clima");
                break;
        }
    }
}
