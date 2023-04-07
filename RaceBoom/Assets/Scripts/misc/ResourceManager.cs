using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;
using DG.Tweening;

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

    static private int lastVal = 0;
    public static void AddCoins(int _arg)
    {
        lastVal = GetCoins();
        SetCoins(GetCoins() + _arg);
        //PlayerPrefs.SetInt("coins", GetCoins() + _arg);
        FindObjectOfType<ResourceManager>().UpdateText();
    }
    public static bool TakeCoins(int _arg)
    {
        lastVal = GetCoins();
        if (GetCoins() < _arg)
        {
            return false;
        }


        SetCoins(GetCoins() - _arg);
        FindObjectOfType<ResourceManager>().UpdateText();
        return true;
    }

    private void UpdateText()
    {
        StartCoroutine( LerpToValue(GetCoins(), 1f));
       
        //resourceText.text = "COINS: " + GetCoins();
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

    private IEnumerator LerpToValue(float endValue, float duration)
    {
        // Store the current value of the slider as the starting value
        float startValue = lastVal;

        // Lerp the slider value and the text value to match the end value
        DOTween.To(() => startValue, x => {
            resourceText.text = AddCommas((int)x);
        }, endValue, duration).SetEase(Ease.OutCirc);

        // Wait for the lerp to finish
        yield return new WaitForSeconds(duration);
    }

    public static string AddCommas(int num)
    {
        if (num < 0)
        {
            return "0";
            //throw new ArgumentException("Number cannot be negative.");
        }
        else if (num < 1000)
        {
            return num.ToString();
        }
        else
        {
            string numStr = num.ToString();
            int numCommas = (numStr.Length - 1) / 3;
            char[] formattedNum = new char[numStr.Length + numCommas];
            int commaPos = 0;
            for (int i = 0; i < numStr.Length; i++)
            {
                if ((numStr.Length - i) % 3 == 0 && i != 0)
                {
                    formattedNum[commaPos++] = ',';
                }
                formattedNum[commaPos++] = numStr[i];
            }
            return new string(formattedNum);
        }
    }
}
