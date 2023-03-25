using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject proj;
    [SerializeField] private Transform shotPoint;

    [SerializeField] private float attackSpeed = 1f;
    [SerializeField] private float initialDelay = 1f;
     
    private void Awake()
    {
        InvokeRepeating(nameof(Shoot), initialDelay, attackSpeed);
    }

    private void Shoot()
    {
        Instantiate(proj, shotPoint.position, shotPoint.rotation);
    }
}
