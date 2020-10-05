using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveAfterAnimation : MonoBehaviour
{
    [SerializeField] private float delay;
    public void Start()
    {
        Destroy(gameObject, delay);
    }
}
