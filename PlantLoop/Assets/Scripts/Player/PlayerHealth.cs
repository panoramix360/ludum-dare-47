using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float initialHealth = 30f;
    [SerializeField] public float health;
    [SerializeField] private Image healthBar;

    private void Start()
    {
        health = initialHealth;
    }

    public void Damage(float amount)
    {
        health -= amount;
        healthBar.fillAmount = health / initialHealth;
    }
}
