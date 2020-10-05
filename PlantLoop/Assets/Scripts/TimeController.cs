using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    private int timeInSeconds = 0;
    private bool canCreateEvent = true;

    [SerializeField] private int timeToUpgrade;
    [SerializeField] private int eventRoutineChance;
    [SerializeField] private int eventRoutineDelay;

    private void Start()
    {
        UnityEngine.Random.InitState(System.DateTime.Now.Millisecond);
    }

    public void OnGameReady()
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
        yield return new WaitForSeconds(1f);

        while (true)
        {
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
        Time.timeScale = 0f;
    }

    public void ResumeTime()
    {
        Time.timeScale = 1f;
    }

    private void EventRoutine()
    {
        if (timeInSeconds % eventRoutineDelay == 0 && canCreateEvent)
        {
            if (Random.Range(1, 101) <= eventRoutineChance)
            {
                GameController.Instance.CreateGameEvent();
                StartCoroutine(DelayToCreateEvent());
            }
        }
    }

    private IEnumerator DelayToCreateEvent()
    {
        canCreateEvent = false;
        yield return new WaitForSeconds(eventRoutineDelay);
        canCreateEvent = true;
    }
}
