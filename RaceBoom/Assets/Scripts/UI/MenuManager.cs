using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class MenuManager : SerializedMonoBehaviour
{

    [SerializeField] private Dictionary<string, CanvasGroup> menus = new Dictionary<string, CanvasGroup>();
    [SerializeField] private GameEventManager gameEventManager = null;
    [SerializeField] private float fadeSpeed = 0.1f;
    [SerializeField] private GameObject[] hideOnGameStart = null;
    private void OnDisable()
    {
        gameEventManager.OnGameOver -= GameEventManager_OnGameOver;
        gameEventManager.OnGameStart -= GameEventManager_OnGameStart;
    }
    void Awake()
    {
        gameEventManager.OnGameOver += GameEventManager_OnGameOver;
        gameEventManager.OnGameStart += GameEventManager_OnGameStart;
    }

    private void Start()
    {
        CloseAllMenus();
        OpenStartMenus();

        GetComponentInChildren<SettingsPanel>(true).LoadPrefs();

        SoundPool.instance.PlayTheme("shop");

    }
    private void ShowAdSkippable()
    {
        //do it until it works
        if (!AdsManager.instance.ShowInterstitialAd())
        {
            //didnt work trying again
            Debug.Log("//didnt work trying again");
            Invoke(nameof(ShowAdSkippable), .2f);
        } else
        {
            Debug.Log("//worked");
        }
    }
    private void ShowAdRewarded()
    {
        //do it until it works
        if (!AdsManager.instance.ShowRewardedAd())
        {
            //didnt work trying again
            Debug.Log("//didnt work trying again");
            Invoke(nameof(ShowAdRewarded), .2f);
        }
        else
        {
            Debug.Log("//worked");
        }
    }

    private void OpenStartMenus()
    {
        OpenMenu("Title");
        OpenMenu("MainMenu");
    }

    public void CloseAllMenus()
    {
        foreach (KeyValuePair<string, CanvasGroup> _k in menus)
        {
            _k.Value.alpha = 0;
            _k.Value.gameObject.SetActive(false);
            /*
            _k.Value.DOFade(0, 0).OnComplete(() => {
                _k.Value.gameObject.SetActive(false);

            });
            */
        }
    }
    public void OpenMenu(string _arg)
    {
        CanvasGroup _k = null;
        menus.TryGetValue(_arg, out _k );

        _k.DOFade(1, fadeSpeed).OnStart(() => {
            _k.gameObject.SetActive(true);

        });
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

    #region events

    private void GameEventManager_OnGameStart()
    {
        CloseAllMenus();
        OpenMenu("Controlls");

        foreach(GameObject _g in hideOnGameStart)
        {
            _g.SetActive(false);
        }
    }
    private void GameEventManager_OnGameOver()
    {
        CloseAllMenus();
        Invoke(nameof(Delayed_OpenGameOver), 2f);
        Invoke(nameof(ShowAdSkippable), 2f);
        //ShowAd();
    }
    private void Delayed_OpenGameOver() => OpenMenu("GameOver");
    public void Button_Continue()
    {
        CloseAllMenus();
        //OpenMenu("Title");
        //OpenMenu("MainMenu");
        Debug.Log("reload scene?!");
        DOTween.Clear(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
    public void Button_ShowRewardedAd()
    {
        ShowAdRewarded();
    }
    public void Button_Start()
    {
        gameEventManager.Notify_OnGameStart();
        SoundPool.instance.PlayRandomTheme();
    }
    public void Button_Shop()
    {
        CloseAllMenus();
        OpenMenu("Shop");
    }
    public void Button_ShopReturn()
    {
        CloseAllMenus();
        OpenStartMenus();

    }
    public void Button_SettingsReturn()
    {
        CloseAllMenus();
        OpenStartMenus();
    }
    public void Button_Settings()
    {
        CloseAllMenus();
        OpenMenu("Settings");

    }

    #endregion

}
