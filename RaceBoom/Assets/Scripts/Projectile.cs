using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float lifetime = 3f;

    void Start()
    {
        GetComponent<Rigidbody>().velocity = Vector3.back * speed;
        Invoke(nameof(DestructionProcess), lifetime);
    }

    private void DestructionProcess()
    {
        transform.DOScale(Vector3.zero, .2f).OnComplete(
            ()=> { Destroy(gameObject); });
            
       
    }

}
