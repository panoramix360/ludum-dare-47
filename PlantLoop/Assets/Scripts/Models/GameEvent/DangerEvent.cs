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
                DurationTime = 5;
                IconPath = "danger_lagarta";
                break;
            case DangerEventType.FUNGUS:
                SetEventModifiers(1f, -0.8f, -0.3f);
                SetEventInstaDamage(0f, 10f, 0f);
                DurationTime = 5;
                IconPath = "danger_fungos";
                break;
            case DangerEventType.FIRE:
                SetEventModifiers(0f, -1f, -0.8f);
                SetEventInstaDamage(0f, 10f, 0f);
                DurationTime = 10;
                IconPath = "danger_queimada";
                break;
            case DangerEventType.GRASSHOPPER:
                SetEventModifiers(0f, -1f, -0.3f);
                SetEventInstaDamage(0f, 10f, 0f);
                DurationTime = 10;
                IconPath = "danger_gafanhoto";
                break;
            case DangerEventType.WEED:
                SetEventModifiers(-0.2f, -0.2f, -0.5f);
                SetEventInstaDamage(0f, 5f, 0f);
                DurationTime = 15;
                IconPath = "danger_erva_daninha";
                break;
            default:
                Debug.LogError("Sem tipo de evento de perigos");
                break;
        }
    }
}
