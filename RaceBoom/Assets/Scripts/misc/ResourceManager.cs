using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;

public class ResourceManager : MonoBehaviour
{
    private TextMeshProUGUI resourceText = null;
    private const string resourceSaveLoc = "resourcesloc";
    // Start is called before the first frame update
    void Start()
    {
        resourceText = GetComponentInChildren<TextMeshProUGUI>();
        UpdateText();
    }

    public static void AddCoins(int _arg)
    {
        SetCoins(GetCoins() + _arg);
        //PlayerPrefs.SetInt("coins", GetCoins() + _arg);
        FindObjectOfType<ResourceManager>().UpdateText();
    }
    public static bool TakeCoins(int _arg)
    {
        if(GetCoins() < _arg)
        {
            return false;
        }


        SetCoins(GetCoins() - _arg);
        FindObjectOfType<ResourceManager>().UpdateText();
        return true;
    }

    private void UpdateText()
    {
        resourceText.text = "COINS: " + GetCoins();
    }

    private static int GetCoins()
    {
        return SaveSystem.LoadIntFromLocation(resourceSaveLoc, 0);
        //return PlayerPrefs.GetInt("coins", 0);
    }
    private static void SetCoins(int _arg)
    {
        SaveSystem.SaveIntAtLocation(_arg, resourceSaveLoc);
        //return PlayerPrefs.GetInt("coins", 0);
    }

    [Button]
    private void Editor_AddCoins(int _a)
    {
        AddCoins(_a);
    }
    
}
