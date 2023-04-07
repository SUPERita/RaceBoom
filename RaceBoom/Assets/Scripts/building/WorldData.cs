using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "WorldData_1", menuName = "WorldData")]
public class WorldData : SerializedScriptableObject
{ 
    [AssetsOnly]
    [field: SerializeField] public GameObject[] parts { get; private set; } = null;
    [field: SerializeField] public int worldStart { get; private set; } = 0;
    [field: SerializeField] public int worldEnd { get; private set; } = 3000;
    [field: SerializeField] public Color worldColor { get; private set; } = Color.white;

    public GameObject GetRandomPart()
    {
        return parts[UnityEngine.Random.Range(0, parts.Length)];
    }
}
