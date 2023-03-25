using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class EquipmentManager : MonoBehaviour
{
    [SerializeField] private Transform headSocket = null;
    //[SerializeField] private Dictionary<string, GameObject> items = new Dictionary<string, GameObject>();


    [SerializeField] private ShopManager shopManager = null;

    private void OnDisable()
    {
        ShopManager.OnChooseItem -= ShopManager_OnChooseItem;
    }
    private void Awake()
    {
        ShopManager.OnChooseItem += ShopManager_OnChooseItem;
    }
    private void Start()
    {
        shopManager.LoadAndHighlightStartingItem();
    }
    private void ShopManager_OnChooseItem(GameObject obj)
    {
        SetHeadEquipment(obj);
        /*
        if (items.TryGetValue(obj, out GameObject _out))
        {
            SetHeadEquipment(_out);
        } else
        {
            SetHeadEquipment(null);
            Debug.Log("no item named:" + obj);
        }
        */
    }

    public void SetHeadEquipment(GameObject _object)
    {
        foreach (Transform child in headSocket)
        {
            Destroy(child.gameObject);
        }
        if (_object)
        {
            Instantiate(_object, headSocket);
        }
    }
}
