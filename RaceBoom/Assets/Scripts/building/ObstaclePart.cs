using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclePart : MonoBehaviour
{
    private Transform trackedObject = null;
    [SerializeField] private float destroyDistance = 15f;
    [SerializeField] private float activateTweensDistance = 30f;
    private bool activatedTweens = false;
    public void SetTrackedObject(Transform _t)
    {
        trackedObject = _t;
    }
    
    void Update()
    {
        if (trackedObject.localPosition.z - destroyDistance > transform.localPosition.z)
        {
            Destroy(gameObject);
        }
        if (!activatedTweens && trackedObject.localPosition.z + activateTweensDistance > transform.localPosition.z)
        {
            TweenObject[] objs = GetComponentsInChildren<TweenObject>();
            foreach(TweenObject _t in objs)
            {
                _t.ActivateTween();
            }
            activatedTweens = true;
        }
    }
}
