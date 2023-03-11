using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleChecker : MonoBehaviour
{
    public event Action<Destructible> OnTriggerDestructible;
    private void OnTriggerEnter(Collider other)
    {
        Destructible _d = null;
        if(other.gameObject.TryGetComponent(out _d))
        {
            OnTriggerDestructible?.Invoke(_d);
        }
    }
}
