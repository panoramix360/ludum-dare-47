using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    private int timeInSeconds = 0;

    private void Start()
    {
        StartCoroutine(BeginTime());
    }

    private IEnumerator BeginTime()
    {
        // Lógica de tempo do jogo
        GameController.Instance.UpdatePlayerAttributesByTimeUnits();

        yield return new WaitForSeconds(1f);
        timeInSeconds++;

        StartCoroutine(BeginTime());
    }
}
