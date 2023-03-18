using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameEventManager_1", menuName = "GameEventManager")]
public class GameEventManager : ScriptableObject
{

    public event Action OnGameOver;
    public void Notify_OnGameOver() => OnGameOver?.Invoke();
    public event Action OnGameStart;
    public void Notify_OnGameStart() => OnGameStart?.Invoke();
    public event Action OnDestructibleDestroyed;
    public void Notify_OnDestructibleDestroyed() => OnDestructibleDestroyed?.Invoke();
}
