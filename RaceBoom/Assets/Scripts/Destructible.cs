using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
public class Destructible : MonoBehaviour
{
    [AssetsOnly]
    [SerializeField] private GameObject[] destroyParticles = null;

    [SerializeField] private bool hasRandomChoiceParticles = false;
    [ShowIf("hasRandomChoiceParticles")]
    [AssetsOnly]
    [SerializeField] private GameObject[] randomChoiceDestroyParticle = null;
    public void Destruct()
    {
        foreach(GameObject _g in destroyParticles)
        {
            Instantiate(_g, transform.position, _g.transform.rotation);
        }
        if (hasRandomChoiceParticles) {
            int rnd = Random.Range(0, randomChoiceDestroyParticle.Length);
            Instantiate(randomChoiceDestroyParticle[rnd], transform.position, randomChoiceDestroyParticle[rnd].transform.rotation);
        }
        Destroy(gameObject);
    }
}
