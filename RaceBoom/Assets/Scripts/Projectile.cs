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
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Collider>().enabled = false;
        transform.DOScale(Vector3.zero, .1f).OnComplete(
            ()=> { Destroy(gameObject); });
            
       
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent(out PlayerMovement _p))
        {
            _p.NotifyColliderWithPlayer();
        }
    }

}
