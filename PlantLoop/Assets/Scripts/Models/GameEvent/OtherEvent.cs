using System.Collections;
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
            case OtherEventType.POORSOILQUALITY:
                SetEventModifiers(-1f, -0.3f, -1f);
                SetEventInstaDamage(0f, 10f, 10f);
                DurationTime = 30;
                IconPath = "event_solo_desgastado";
                break;
            case OtherEventType.GOODSOILQUALITY:
                SetEventModifiers(0.5f, 0.5f, 0.5f);
                SetEventInstaDamage(5f, 5f, 5f);
                DurationTime = 30;
                IconPath = "event_solo_fertil";
                break;
            default:
                Debug.LogError("Sem tipo de evento Outros");
                break;
        }
    }
}
