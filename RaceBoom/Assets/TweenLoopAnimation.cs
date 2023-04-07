using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;
public class TweenLoopAnimation : MonoBehaviour
{
    [SerializeField] private bool bobUp = false;
    [ShowIf(nameof(bobUp))]
    [SerializeField] private float bobHeight = 20f;
    [ShowIf(nameof(bobUp))]
    [SerializeField] private float bobDuration = 1f;
    [ShowIf(nameof(bobUp))]
    [SerializeField] private LoopType bobLoopType = LoopType.Yoyo;

    [SerializeField] private bool tweenScale = false;
    [ShowIf(nameof(tweenScale))]
    [SerializeField] private float scaleAmount = 1.25f;
    [ShowIf(nameof(tweenScale))]
    [SerializeField] private float scaleDuration = 1f;
    [ShowIf(nameof(tweenScale))]
    [SerializeField] private LoopType scaleLoopType = LoopType.Yoyo;

    [SerializeField] private bool tweenRotation = false;
    [ShowIf(nameof(tweenRotation))]
    [SerializeField] private float rotationAmount = 45f;
    [ShowIf(nameof(tweenRotation))]
    [SerializeField] private float rotationDuration = 1f;
    [ShowIf(nameof(tweenRotation))]
    [SerializeField] private LoopType rotationLoopType = LoopType.Yoyo;
    void Start()
    {
        if (bobUp)
        {
            transform.DOMoveY(transform.position.y + bobHeight, bobDuration / 2.0f).SetEase(Ease.OutQuad)
            .SetLoops(-1, bobLoopType);
        }
        if (tweenScale)
        {
            transform.DOScale(new Vector3(transform.localScale.x + scaleAmount, transform.localScale.y + scaleAmount, transform.localScale.z + scaleAmount), scaleDuration / 2.0f).SetEase(Ease.OutQuad)
            .SetLoops(-1, scaleLoopType);
        }
        if (tweenRotation)
        {
            transform.DORotate(new Vector3(0.0f, 0f, rotationAmount), rotationDuration).SetEase(Ease.Linear)
           .SetLoops(-1, rotationLoopType);
        }


    }

    
}
