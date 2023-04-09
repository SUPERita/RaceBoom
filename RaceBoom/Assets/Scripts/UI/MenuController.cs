using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class MenuController : SerializedMonoBehaviour
{

    [SerializeField] private Dictionary<string, CanvasGroup> menus = new Dictionary<string, CanvasGroup>();
    [SerializeField] private float fadeSpeed = 0.1f;
    string currentMenuName = "empty";

    private void Start()
    {
        if(currentMenuName == "empty")
        {
            OpenMenu("hats");
        }
    }

    public void CloseAllMenus()
    {
        foreach (KeyValuePair<string, CanvasGroup> _k in menus)
        {
            _k.Value.DOFade(0, 0).OnComplete(() => {
                _k.Value.gameObject.SetActive(false);

            });
        }
    }
    public void OpenMenu(string _arg)
    {
        //Debug.Log("called oprn menu with " + _arg);
        CloseAllMenus();
        CanvasGroup _k = null;
        menus.TryGetValue(_arg, out _k);

        _k.DOFade(1, fadeSpeed).OnStart(() => {
            _k.gameObject.SetActive(true);

        });

        currentMenuName = _arg;
    }
    public void CloseMenu(string _arg)
    {
        CanvasGroup _k = null;
        menus.TryGetValue(_arg, out _k);

        _k.DOFade(0, fadeSpeed).OnComplete(() => {
            _k.gameObject.SetActive(false);

        });

    }

    [HorizontalGroup("g1", Width = .5f)]
    [Button]
    public void Editor_OpenMenu(string _arg)
    {
        CanvasGroup _k = null;
        menus.TryGetValue(_arg, out _k);
        _k.alpha = 1;
        _k.gameObject.SetActive(true);

    }
    [HorizontalGroup("g1", Width = .5f)]
    [Button]
    public void Editor_CloseAllMenus()
    {
        foreach (KeyValuePair<string, CanvasGroup> _k in menus)
        {
            _k.Value.alpha = 0;
            _k.Value.gameObject.SetActive(false);
        }
    }

    

}
