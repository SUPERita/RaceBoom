using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class TweenObject : MonoBehaviour
{
    [SerializeField] private Vector3 moveVector;
    [SerializeField] private float duration = 1f;
    [SerializeField] private float delayAfterActivation = 0f;

    public void ActivateTween()
    {
        Invoke(nameof(DoTheTween), delayAfterActivation);
    }

    private void DoTheTween()
    {
        transform.DOLocalMove(transform.localPosition + moveVector, duration);
    }

}
