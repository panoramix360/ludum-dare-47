using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    private int timeInSeconds = 0;

    [SerializeField] private int timeToUpgrade;
    [SerializeField] private int eventRoutineChance;

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

        EventRoutine();

        yield return new WaitForSeconds(1f);
        timeInSeconds++;

        StartCoroutine(BeginTime());
    }

    private void EventRoutine()
    {
        System.Random rnd = new System.Random();
        if (rnd.Next(100) <= eventRoutineChance)
        {
            Debug.Log("Criando evento");
            GameController.Instance.CreateGameEvent();
        }
        //COMENTEI PRA SER MAIS FÁCIL PRA TESTAR
        //if (timeInSeconds == 10)
        //{
        //    //if (rnd.Next(100) <= 10)
        //    //{
        //        GameController.Instance.CreateGameEvent();
        //    //}
        //} 
        //else if(timeInSeconds == 30)
        //{
        //    //if (rnd.Next(100) <= 95)
        //    //{
        //        GameController.Instance.CreateGameEvent();
        //    //}
        //}
        //else if (timeInSeconds == 50)
        //{
        //    //if (rnd.Next(100) <= 95)
        //    //{
        //        GameController.Instance.CreateGameEvent();
        //    //}
        //}
        //else if (timeInSeconds == 80)
        //{
        //    //if (rnd.Next(100) <= 95)
        //    //{
        //        GameController.Instance.CreateGameEvent();
        //    //}
        //}
        //else if (timeInSeconds == 100)
        //{
        //    //if (rnd.Next(100) <= 95)
        //    //{
        //        GameController.Instance.CreateGameEvent();
        //    //}
        //}
        //else if (timeInSeconds == 120)
        //{
        //    //if (rnd.Next(100) <= 95)
        //    //{
        //        GameController.Instance.CreateGameEvent();
        //    //}
        //}
    }
}
