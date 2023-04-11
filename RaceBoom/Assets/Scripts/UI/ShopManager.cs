using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using System;

public class ShopManager : MonoBehaviour
{
    //ShopName used for saving
    [SerializeField] private string ShopName = "noName";
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
        //if owned
        if (shopItemData.GetIsPurchesed())
        {
            SetHighlight(_s);
            SoundPool.instance.PlaySound("item_equip");
        }
        //if has enougth money, buy
        else if (ResourceManager.TakeCoins(shopItemData._price))
        {
            MessageManager.instance.SendMessage("UNLOCKED: " + shopItemData._itemName + "!!!");
            _s.SetOpen(true);
            shopItemData.SetIsPurchesed(true);
            SetHighlight(_s);
            SoundPool.instance.PlaySound("item_unlocked");
        }
        //too expensive
        else
        {
            SoundPool.instance.PlaySound("item_locked");
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
        SaveSystem.SaveIntAtLocation(_arg, ShopName+highlightSaveLoc);
    }
    private int GetHighlightSave()
    {
        return SaveSystem.LoadIntFromLocation(ShopName + highlightSaveLoc, 0); 
    }

    #endregion

}

[System.Serializable]
public struct ShopItemData
{
    private const string ItemSaveLocPrefix = "itemLoc";
    [LabelWidth(30)]
    [HorizontalGroup("g2")]
    [PreviewField]
    public Sprite _image;

    [HorizontalGroup("g2")]
    [LabelWidth(30)]
    //[HorizontalGroup("g")]
    [PreviewField]
    [AssetsOnly]
    //[AssetSelector]
    public GameObject _item;

    [LabelWidth(70)]
    [HorizontalGroup("g")]
    public string _itemName;
    [LabelWidth(70)]
    [HorizontalGroup("g")]
    public int _price;


    public bool GetIsPurchesed() { return SaveSystem.LoadBoolFromLocation(ItemSaveLocPrefix+_itemName, false)/*PlayerPrefs.GetInt("ShopItemData" + _itemName,0) == 1? true : false*/; }
    public void SetIsPurchesed(bool _arg) { SaveSystem.SaveBoolAtLocation(_arg, ItemSaveLocPrefix + _itemName)/*PlayerPrefs.SetInt("ShopItemData" + _itemName, _arg ? 1 : 0)*/; }
    //public bool isPurchesed;
}
