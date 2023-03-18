using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    [SerializeField] private Health health = null;
    [SerializeField] private Image healthImage = null;


    private void Start()
    {
        health.OnHealthChanged += Health_OnHealthChanged;
    }
    private void OnDisable()
    {
        health.OnHealthChanged -= Health_OnHealthChanged;
    }
    private void Health_OnHealthChanged(int obj)
    {
        healthImage.fillAmount = obj / health.maxHealth;
    }


    [Button]
    private void Editor_CacheHealth()
    {
        health = GetComponent<Health>();
    }
}
