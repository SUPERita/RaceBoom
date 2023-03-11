using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    [SerializeField] private GameObject destroyParticle = null;
    public void Destruct()
    {
        Instantiate(destroyParticle, transform.position, destroyParticle.transform.rotation);
        Destroy(gameObject);
    }
}
