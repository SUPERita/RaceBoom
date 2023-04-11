using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

public class Destructible : MonoBehaviour
{
    [SerializeField] private string deathSound = "break";
    [AssetsOnly]
    [SerializeField] private GameObject[] destroyParticles = null;

    [SerializeField] private bool hasRandomChoiceParticles = false;
    [ShowIf("hasRandomChoiceParticles")]
    [AssetsOnly]
    [SerializeField] private GameObject[] randomChoiceDestroyParticle = null;

    public event Action OnDestructNotify;
    public void Destruct()
    {
        SoundPool.instance.PlaySound(deathSound);
        foreach (GameObject _g in destroyParticles)
        {
            Instantiate(_g, transform.position, _g.transform.rotation);
        }
        if (hasRandomChoiceParticles) {
            int rnd = UnityEngine.Random.Range(0, randomChoiceDestroyParticle.Length);
            Instantiate(randomChoiceDestroyParticle[rnd], transform.position, randomChoiceDestroyParticle[rnd].transform.rotation);
        }
        Destroy(gameObject);
    }

    public void Notify_ToDistruct()
    {
        OnDestructNotify?.Invoke();
    }
}
