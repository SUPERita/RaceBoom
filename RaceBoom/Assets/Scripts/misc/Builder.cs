using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Builder : MonoBehaviour
{

    [SerializeField] private GameObject[] parts = null;
    [SerializeField] private int partDistance = 15;
    [SerializeField] private Transform trackedObject = null;
    [SerializeField] private int objectBuildAhead = 30;
    [SerializeField] private int skipFirstParts = 1;
    private int count = 0;
    [Header("object fading")]
    [SerializeField] private int fadeFromDistance = -7;
    [SerializeField] private float fadeTime = 1f;
    [SerializeField] private float fadePartDelay = .1f;
    [SerializeField] private Ease fadeEase = Ease.Flash;
    
    void Update()
    {
        if (trackedObject.localPosition.z+ objectBuildAhead > count * partDistance)
        {
            if (count > skipFirstParts) { SpawnRandomPart(); }
            count++;
        }
    }

    private void SpawnRandomPart()
    {
        GameObject _randomPart = parts[UnityEngine.Random.Range(0, parts.Length)];
        GameObject _tmp = Instantiate(_randomPart, Vector3.forward * count*partDistance, Quaternion.identity);
        Transform[] _children = _tmp.GetComponentsInChildren<Transform>();
        int _subTransformCount = 0;
        foreach(Transform _t in _children)
        {
            Vector3 _startPos = _t.localPosition;
            _t.localPosition = _startPos - (Vector3.up * fadeFromDistance);
            _t.DOLocalMove(_startPos, fadeTime + (_subTransformCount/(10*fadeTime))).SetEase(fadeEase);

            _subTransformCount++;
        }
    }
}
