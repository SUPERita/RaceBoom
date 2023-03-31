using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class EquipmentManager : MonoBehaviour
{
    [SerializeField] private Transform headSocket = null;
    [SerializeField] private Transform skinSocket = null;
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
        if(obj == null) {
            //"reset the sockets"
            //SetEquipment(obj, skinSocket);
            SetEquipment(obj, headSocket);
            return; 
        }
        switch (obj.GetComponent<Cosmetic>().cosmeticType)
        {
            case (Cosmetic.CosmeticType.Hat):
                SetEquipment(obj, headSocket);
                break;
            case (Cosmetic.CosmeticType.Skin):
                SetEquipment(obj, skinSocket);
                break;
        }

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

    public void SetEquipment(GameObject _object, Transform _socket)
    {
        foreach (Transform child in _socket)
        {
            Destroy(child.gameObject);
        }
        if (_object)
        {
            Instantiate(_object, _socket);
        }
    }
}
