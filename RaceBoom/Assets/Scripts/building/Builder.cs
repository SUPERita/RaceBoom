using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class Builder : MonoBehaviour
{

    [SerializeField] public WorldController worldController = null;
    //private int currentWorld = 0;
    [SerializeField] private int partDistance = 15;
    [SerializeField] private Transform trackedObject = null;
    [SerializeField] private int objectBuildAhead = 30;
    [SerializeField] private int skipFirstParts = 1;
    [SerializeField] private GameEventManager gameEventManager = null;
    private int count = 0;
    //[Header("object fading")]
    [FoldoutGroup("object fading")]
    [SerializeField] private int fadeFromDistance = -7;
    [FoldoutGroup("object fading")]
    [SerializeField] private float fadeTime = 1f;
    [FoldoutGroup("object fading")]
    [SerializeField] private float fadePartDelay = .1f;
    [FoldoutGroup("object fading")]
    [SerializeField] private Ease fadeEase = Ease.Flash;
    [FoldoutGroup("object fading")]
    [SerializeField] private Transform endHider = null;
    [FoldoutGroup("object fading")]
    [SerializeField] private float endHiderAddition = 5f;
    private bool started = false;

    private void Start()
    {
        gameEventManager.OnGameStart += GameEventManager_OnGameStart;

        //currentWorld = GetCurrentWorld();
    }
    private void OnDisable()
    {
        gameEventManager.OnGameStart -= GameEventManager_OnGameStart;
    }

    private void GameEventManager_OnGameStart()
    {
        started = true;
    }

    void Update()
    {
        if (!started) { return; }
        if (trackedObject.localPosition.z+ objectBuildAhead > count * partDistance)
        {
            if (count > skipFirstParts) {
                endHider.localPosition = endHider.localPosition + Vector3.forward * endHiderAddition;
                SpawnRandomPart();
            }
            count++;
        }
    }

    private void SpawnRandomPart()
    {
        GameObject _randomPart = worldController.GetRandomPartFromCurrentWorld();
        GameObject _instantiatedPart = Instantiate(_randomPart, Vector3.forward * count*partDistance, _randomPart.transform.rotation);
        Transform[] _children = _instantiatedPart.GetComponentsInChildren<Transform>();

        Vector3 _startPos = _instantiatedPart.transform.localPosition;
        _instantiatedPart.transform.localPosition = _startPos - (Vector3.up * fadeFromDistance);
        _instantiatedPart.transform.DOLocalMove(_startPos, fadeTime).SetEase(fadeEase);

        _instantiatedPart.GetComponent<ObstaclePart>().SetTrackedObject(trackedObject);
        //Destroy(_instantiatedPart, 10f);
        /*
        int _subTransformCount = 0;
        foreach(Transform _t in _children)
        {
            Vector3 _startPos = _t.localPosition;
            _t.localPosition = _startPos - (Vector3.up * fadeFromDistance);
            _t.DOLocalMove(_startPos, fadeTime 
                + _t.childCount < 3 ? (_subTransformCount/(10*fadeTime)) : 0
                ).SetEase(fadeEase);

            _subTransformCount++;
            if(_subTransformCount > 3) { _subTransformCount = 0; }
        }
        */
    }

    
}
