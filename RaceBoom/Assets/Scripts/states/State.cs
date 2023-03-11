using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "BaseState", menuName = "State/Base")]

public abstract class State : ScriptableObject
{
    [field: SerializeField] public string stateName { get; private set; } = "sampleName";
    [field: SerializeField] public int priotiry { get; private set; } = 0;
    public abstract void OnUpdate();
    public abstract void OnEnter(GameObject _g);
    public abstract void OnExit();

}
