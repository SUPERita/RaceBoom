using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [field: SerializeField] public int maxHealth { get; private set; } = 100;
    private int currentHealth = 0;

    public event Action<int> OnHealthChanged;
    public event Action OnDie;
    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int _amount)
    {
        if(currentHealth == 0) { return; }
        currentHealth = Mathf.Clamp(currentHealth - _amount, 0,maxHealth);

        OnHealthChanged?.Invoke(_amount);
        if(currentHealth == 0) { OnDie?.Invoke(); }
    }
    public void AddHealth(int _amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + _amount, 0, maxHealth);

        OnHealthChanged?.Invoke(_amount);
    }
}
