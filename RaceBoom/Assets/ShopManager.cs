using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using System;

public class ShopManager : MonoBehaviour
{

    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private ShopItemData[] items = null;

    [SerializeField] private RectTransform itemParent = null;

    private const string highlightSaveLoc = "highlightLoc";
    public static event Action<GameObject> OnChooseItem;

    private bool loaded = false;
    void Awake()
    {
        if (!loaded)
        {
            LoadAndHighlightStartingItem();
        }
    }

    public void LoadAndHighlightStartingItem()
    {
        foreach (ShopItemData _item in items)
        {
            ShopItem _g = Instantiate(buttonPrefab, itemParent).GetComponent<ShopItem>()
                .SetImage(_item._image)
                .SetName(_item._itemName)
                .SetPrice(_item._price)
                .SetOpen(_item.GetIsPurchesed())
                .SetHighlight(false);
            _g.gameObject.GetComponent<Button>().onClick.AddListener(() => OnButtonClick(_g));
        }
        SetHighlight(ItemDataToItem(items[GetHighlightSave()/*PlayerPrefs.GetInt("savedHighlightIndex", 0)*/]));
        loaded = true;
    }

    public void OnButtonClick(ShopItem _s)
    {
        ShopItemData shopItemData = ItemToItemData(_s);
        if (shopItemData.GetIsPurchesed())
        {
            SetHighlight(_s);
            //Debug.Log("2");
        }
        else if (ResourceManager.TakeCoins(shopItemData._price))
        {
            //buy item
           // Debug.Log("1");
            _s.SetOpen(true);
            shopItemData.SetIsPurchesed(true);
            SetHighlight(_s);
        }
    }


    private void SetHighlight(ShopItem _arg)
    {
        ShopItem[] _items = GetComponentsInChildren<ShopItem>();
        int _i = 0;
        foreach(ShopItem _item in _items)
        {

            if(_item == _arg)
            {
                _item.SetHighlight(true);
                //PlayerPrefs.SetInt("savedHighlightIndex", _i);
                Debug.Log(_item.name);
                OnChooseItem?.Invoke(ItemToItemData(_item)._item);
                SetHighlightSave(_i);
            } else
            {
                _item.SetHighlight(false);
            }
            _i++;
        }
       
    }

    public ShopItemData ItemToItemData(ShopItem _s)
    {
        foreach(ShopItemData _data in items)
        {
            if(_data._itemName == _s.GetItemName())
            {
                return _data;
            }
        }
        Debug.LogError("no item named " + _s.GetItemName());
        return items[0];
    }
    public ShopItem ItemDataToItem(ShopItemData _s)
    {
        ShopItem[] _shopItems= GetComponentsInChildren<ShopItem>();
        foreach (ShopItem _shopitem in _shopItems)
        {
            if (_shopitem.GetItemName() == _s._itemName)
            {
                return _shopitem;
            }
        }
        Debug.LogError("no item named " + _s._itemName);
        return _shopItems[0];
    }

    [Button]
    private void Editor_ResetAllSaves()
    {
        foreach(ShopItemData _s in items)
        {
            _s.SetIsPurchesed(false);
        }
        //PlayerPrefs.SetInt("savedHighlightIndex", 0);
        SetHighlightSave(0);
    }

    #region saving

    private void SetHighlightSave(int _arg)
    {
        SaveSystem.SaveIntAtLocation(_arg, highlightSaveLoc);
    }
    private int GetHighlightSave()
    {
        return SaveSystem.LoadIntFromLocation(highlightSaveLoc, 0); 
    }

    #endregion

}

[System.Serializable]
public struct ShopItemData
{
    private const string ItemSaveLocPrefix = "itemLoc";
    [PreviewField(50)]
    public Sprite _image;
    public string _itemName;
    public int _price;
    [PreviewField(50)]
    [AssetsOnly]
    [AssetSelector]
    public GameObject _item;

    public bool GetIsPurchesed() { return SaveSystem.LoadBoolFromLocation(ItemSaveLocPrefix+_itemName, false)/*PlayerPrefs.GetInt("ShopItemData" + _itemName,0) == 1? true : false*/; }
    public void SetIsPurchesed(bool _arg) { SaveSystem.SaveBoolAtLocation(_arg, ItemSaveLocPrefix + _itemName)/*PlayerPrefs.SetInt("ShopItemData" + _itemName, _arg ? 1 : 0)*/; }
    //public bool isPurchesed;
}
