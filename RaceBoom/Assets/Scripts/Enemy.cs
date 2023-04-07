using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject proj;
    [SerializeField] private Transform shotPoint;
    private Animator anim = null;
    [SerializeField] private float attackDelay = 1f;
    [SerializeField] private float initialDelay = 1f;
    [SerializeField] private float spawnChance = 100f;
    private CancellationTokenSource cancellationTokenSource;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    private void Start()
    {
        cancellationTokenSource = new CancellationTokenSource();
        StartStateControl();

        if (!GetChance(spawnChance))
        {
            Destroy(gameObject);
        }
    }

    private async void StartStateControl()
    {
        await Idle(initialDelay);
        StateControl(cancellationTokenSource.Token);
    }

    private async void StateControl(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            //Debug.Log("no token");
            await Attack();
            await Idle(attackDelay);
        }
        
    }

    private async Task Attack()
    {
        if (!anim) { return; }

        anim.CrossFadeInFixedTime("Attack", .2f);
        await Task.Delay((int)(1 * 1000f));

    }
    private async Task Idle(float secondsToWait)
    {
        if (!anim) { return; }

        anim.CrossFadeInFixedTime("Idle", .2f);
        await Task.Delay((int)(secondsToWait * 1000f));
    }

    public void Anim_Shoot()
    {
        Instantiate(proj, shotPoint.position, shotPoint.rotation);
    }

    private void OnDisable()
    {
        cancellationTokenSource.Cancel();
    }

    /// <summary>
    /// 0%-100% in precentages
    /// </summary>
    /// <param name="precentage"></param>
    /// <returns></returns>
    private bool GetChance(float precentage)
    {
        return UnityEngine.Random.Range(0f, 100f) < precentage;
    }
}
