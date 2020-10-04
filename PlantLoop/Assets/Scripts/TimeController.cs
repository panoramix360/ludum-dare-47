using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    private int timeInSeconds = 0;

    [SerializeField] private int timeToUpgrade;

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
        // Lógica de tempo do jogo
        GameController.Instance.UpdatePlayerAttributesByTimeUnits();

        if (CheckIfIsTime(timeToUpgrade))
        {
            GameController.Instance.UpgradePlayerNode();
        }

        yield return new WaitForSeconds(1f);
        timeInSeconds++;

        StartCoroutine(BeginTime());
    }
}
