using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Cosmetic : MonoBehaviour
{
    public enum CosmeticType { Hat, Skin }

    [field:SerializeField] public CosmeticType cosmeticType { get; private set; } = CosmeticType.Hat;
}
