using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusEvent : GameEvent
{
    public BonusEvent(GameEvent gameEvent) : base(gameEvent)
    {
        GetEventModifiers();
    }

    private void GetEventModifiers()
    {
        switch (BonusType)
        {
            case BonusEventType.BEES:
                SetEventModifiers(0f, 1f, 0f);
                SetEventInstaDamage(0f, 3f, 0f);
                DurationTime = 10;
                IconPath = "bonus_abelha";
                break;
            case BonusEventType.LADYBUG:
                SetEventModifiers(0.5f, 1f, 0f);
                SetEventInstaDamage(0f, 10f, 0f);
                DurationTime = 10;
                IconPath = "bonus_joaninha";
                break;
            case BonusEventType.WORM:
                SetEventModifiers(1f, 0f, 1f);
                SetEventInstaDamage(10f, 0f, 10f);
                DurationTime = 10;
                IconPath = "bonus_minhoca";
                break;
            default:
                Debug.LogError("Sem tipo de evento de bonus");
                break;
        }

        BeginCoroutine();
    }
}
