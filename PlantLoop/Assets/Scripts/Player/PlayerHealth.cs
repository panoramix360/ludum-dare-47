using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private Image healthBar;

    public void Damage(int amount)
    {
        healthBar.fillAmount -= amount;
    }
}
