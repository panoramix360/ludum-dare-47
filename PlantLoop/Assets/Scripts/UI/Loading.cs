using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Loading : MonoBehaviour
{
    [SerializeField] private UnityEvent OnFadeIn;
    [SerializeField] private UnityEvent OnFadeOut;

    public void OnFinishFadeIn()
    {
        OnFadeIn.Invoke();
    }

    public void OnFinishFadeOut()
    {
        OnFadeOut.Invoke();
    }
}
