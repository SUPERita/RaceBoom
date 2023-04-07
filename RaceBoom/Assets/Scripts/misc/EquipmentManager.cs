using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class EquipmentManager : MonoBehaviour
{
    [Header("Skin")]
    [SerializeField] private Transform skinSocket = null;
    [SerializeField] private Transform rootBone = null;
    [Header("Hat")]
    [SerializeField] private Transform headSocket = null;
    //[SerializeField] private Dictionary<string, GameObject> items = new Dictionary<string, GameObject>();

    [SerializeField] private ShopManager[] shopManagers = null;
    [Header("VFX")]
    [AssetsOnly]
    [SerializeField] private GameObject equipVFX = null;

    private void Start()
    {
        foreach(ShopManager _s in shopManagers)
        {
            _s.LoadAndHighlightStartingItem();
        }
    }

    private void OnDisable()
    {
        ShopManager.OnChooseItem -= ShopManager_OnChooseItem;
    }
    private void Awake()
    {
        ShopManager.OnChooseItem += ShopManager_OnChooseItem;
    }

    private void ShopManager_OnChooseItem(GameObject obj)
    {
        if(obj == null) {
            //"reset the sockets"
            //SetEquipment(obj, skinSocket);
            DestroyChildren(headSocket);
            //SetEquipment(obj, headSocket);
            return; 
        }
        switch (obj.GetComponent<Cosmetic>().cosmeticType)
        {
            case (Cosmetic.CosmeticType.Hat):
                SetEquipment(obj, headSocket);
                break;
            case (Cosmetic.CosmeticType.Skin):
                SetEquipment(obj, skinSocket);
                obj.GetComponent<SkinnedMeshRenderer>().rootBone = rootBone;
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
        if (!_object) { return; }
        Instantiate(equipVFX, transform.position + Vector3.up * 1f, equipVFX.transform.rotation);
        StartCoroutine(UpdateEquipmentRender(_object, _socket));
        //UpdateEquipmentRender(_object, _socket);
    }

    private IEnumerator UpdateEquipmentRender(GameObject _object, Transform _socket)
    {
        yield return new WaitForSeconds(.25f);
        Cosmetic.CosmeticType _c = _object.GetComponent<Cosmetic>().cosmeticType;
        //if hat
        if (_c == Cosmetic.CosmeticType.Hat)
        {
            DestroyChildren(_socket);
            if (_object)
            {
                Instantiate(_object, _socket);
            }
        }
        //if skin
        if (_c == Cosmetic.CosmeticType.Skin)
        {
            DisableChildren(_socket);
            if (_object)
            {
                foreach (Transform child in _socket)
                {
                    if (child.gameObject.GetComponent<Cosmetic>().cosmeticIdentifier == _object.GetComponent<Cosmetic>().cosmeticIdentifier)
                    {
                        child.gameObject.SetActive(true);
                    }

                }
            }
        }
    }

    private void DestroyChildren(Transform _socket)
    {
        foreach (Transform child in _socket)
        {
            Destroy(child.gameObject);
        }
    }
    private void DisableChildren(Transform _socket)
    {
        foreach (Transform child in _socket)
        {
            child.gameObject.SetActive(false);
        }
    }
}
