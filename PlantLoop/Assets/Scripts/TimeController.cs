using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    private int timeInSeconds = 0;
    private bool isPaused = false;
    private bool canCreateEvent = true;

    [SerializeField] private int timeToUpgrade;
    [SerializeField] private int eventRoutineChance;
    [SerializeField] private int eventRoutineDelay;

    private void Start()
    {
        StartCoroutine(BeginTime());
    }

    private bool CheckIfIsTime(int timeToCheck)
    {
        if (timeInSeconds == 0) return false;
        return timeInSeconds % timeToCheck == 0;
    }

    private IEnumerator BeginTime()
    {
        while (true)
        {
            while (isPaused)
            {
                yield return null;
            }

            // Lógica de tempo do jogo
            GameController.Instance.UpdatePlayerAttributesByTimeUnits();

            if (CheckIfIsTime(timeToUpgrade))
            {
                GameController.Instance.UpgradePlayerNode();
            }

            EventRoutine();

            yield return new WaitForSeconds(1f);
            timeInSeconds++;
        }
    }

    public void PauseTime()
    {
        isPaused = true;
    }

    public void ResumeTime()
    {
        isPaused = false;
    }

    private void EventRoutine()
    {
        System.Random rnd = new System.Random();
        if (timeInSeconds % eventRoutineDelay == 0 && canCreateEvent)
        {
            if (rnd.Next(100) <= eventRoutineChance)
            {
                GameController.Instance.CreateGameEvent();
                StartCoroutine(DelayToCreateEvent());
            }
        }
    }

    private IEnumerator DelayToCreateEvent()
    {
        Debug.Log("canCreateEvent entrando");
        canCreateEvent = false;
        yield return new WaitForSeconds(eventRoutineDelay);
        canCreateEvent = true;
        Debug.Log("canCreateEvent saindo");
    }
}
