using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class ShopItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemPrice = null;
    [SerializeField] private TextMeshProUGUI itemName = null;
    [SerializeField] private Image itemImage = null;
    [SerializeField] private Image closedImage = null;
    [SerializeField] private Image highlightImage = null;

    //maye also keep the price data here
    public ShopItem SetPrice(int _arg)
    {

        itemPrice.text = _arg.ToString();
        return this;
    }
    public ShopItem SetName(string _arg)
    {
        itemName.text = _arg;
        return this;
    }
    public ShopItem SetImage(Sprite _arg)
    {
        itemImage.sprite = _arg;
        return this;
    }
    public ShopItem SetOpen(bool _arg) {
        closedImage.gameObject.SetActive(!_arg);
        //GetComponent<Button>().interactable = _arg;
        itemPrice.enabled = !_arg;
        return this;
    }
    public ShopItem SetHighlight(bool _arg)
    {
        highlightImage.enabled = _arg;
        return this;
    }

    public string GetItemName()
    {
        return itemName.text.ToString();
    }

    

    private void OnDestroy()
    {
        GetComponent<Button>().onClick.RemoveAllListeners();
    }
}
