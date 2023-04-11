using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Sirenix.OdinInspector;
using DG.Tweening;

[System.Serializable]
public struct SFX
{
    public string _name;
    public AudioClip clip;
    [Tooltip("0-1")]
    public float _volume;
    [Tooltip("0-2?")]
    public float _pitch;
}

public class SoundPool : MonoBehaviour
{
    public static SoundPool instance = null;

    private AudioSource _source = null;
    private List<Sound> activeSounds = new List<Sound>();
    private List<Sound> inactiveSounds = new List<Sound>();
     
    [Header("sfx")]
    [SerializeField] private GameObject samplePoolSound = null;
    [SerializeField] private int startingPoolObjects = 20;
    //[AssetList(Path = "/Audio", AutoPopulate = true)]
    private SFX[] allClips = null;
    [TabGroup("UI")]
    [SerializeField] private SFX[] UIClips;
    [TabGroup("Player")]
    [SerializeField] private SFX[] PlayerClips;
    [TabGroup("Enemeis")]
    [SerializeField] private SFX[] EnemiesClips;

    [Header("Themes")]
    [SerializeField] private SFX[] themes = null;
    [InfoBox("nonRandomThemes cant be randomly played with the PlayRandomTheme funtion")]
    [SerializeField] private string[] nonRandomThemes = null;
    [SerializeField] private AudioSource themeSource = null;
    private SFX currentTheme;

    void Awake()
    {
        List<SFX> sfxsTemp = new List<SFX>();
        foreach (SFX _ss in UIClips) { sfxsTemp.Add(_ss); }
        foreach (SFX _ss in PlayerClips) { sfxsTemp.Add(_ss); }
        foreach (SFX _ss in EnemiesClips) { sfxsTemp.Add(_ss); }
        allClips = sfxsTemp.ToArray();

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.Log("there was another soundpool");
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InitializeStartingPoolObjects();
    }


    public AudioSource PlaySound(string _clipName, float pitchVaritaion = 0,float destroyInSeconds = 2, bool isLoop = false)
    {
        if (inactiveSounds.Count != 0)
        {
            Sound tmpSound = inactiveSounds[0];
            SFX _sfx = GetAudioClipByName(_clipName);
            tmpSound._source.clip = _sfx.clip;
            tmpSound._source.volume = _sfx._volume;
            tmpSound._source.pitch = _sfx._pitch + pitchVaritaion * UnityEngine.Random.Range(-1f, 1f);
            tmpSound._source.Play();
            tmpSound._source.loop = isLoop;
            activeSounds.Add(tmpSound);
            inactiveSounds.Remove(tmpSound);
            //set a timer of a second to make it inactive
            if (!isLoop)
            {
                StartCoroutine(ReturnSoundToPool(tmpSound, destroyInSeconds));
            }
            return tmpSound._source;

        }
        else
        {
            //dono
            return null;
        }

    }
    [Button]
    public void PlayTheme(string _themeName)
    {
        currentTheme = GetThemeByName(_themeName);
        if (themeSource.clip == GetThemeByName(_themeName).clip) { return; }

        themeSource.DOFade(0, .25f).OnComplete(() =>
        {
            themeSource.clip = GetThemeByName(_themeName).clip;
            themeSource.DOFade(GetThemeByName(_themeName)._volume/2f, .25f);
            themeSource.Play();
        });
        //needs tweening
    }
    [Button]
    public void PlayRandomTheme()
    {
        string _rand = themes[UnityEngine.Random.Range(0, themes.Length)]._name;
        //string[] _allThemes = SFXArrToStringArr(themes);

        foreach(string _s in nonRandomThemes)
        {
            if(_rand == _s)
            {
                PlayRandomTheme();
                return;
            }
        }

        themeSource.loop = true;
        PlayTheme(_rand);
    }

    [Button]
    public void ExantuateTheme(float _maxPeak = 1f, float _attk = .2f, float _fall = 1f)
    {
        themeSource.DOKill();
        float _startVol = currentTheme._volume;
        themeSource.DOFade(_maxPeak, _attk)
            .OnComplete(
                ()=> themeSource.DOFade(_startVol/2f, _fall)
            );
    }

    #region Pool Methods

    private void InitializeStartingPoolObjects()
    {
        for (int i = 0; i < startingPoolObjects; i++)
        {
            GameObject g = Instantiate(samplePoolSound, transform);
            inactiveSounds.Add(g.GetComponent<Sound>());
        }
    }

    private IEnumerator ReturnSoundToPool(Sound _s, float destroyDelay)
    {
        yield return new WaitForSeconds(destroyDelay);

        _s._source.clip = null;

        inactiveSounds.Add(_s);
        activeSounds.Remove(_s);
    }

    #endregion

    private SFX GetAudioClipByName(string _name)
    {
        foreach (SFX s in allClips)
        {
            //Debug.Log(s._name);
            if (s._name.Equals(_name))
            {
                return s;
            }
        }
        Debug.LogError("no sound named: " + _name);
        return new SFX();
    }
    private SFX GetThemeByName(string _name)
    {
        foreach (SFX s in themes)
        {
            if (s._name.Equals(_name))
            {
                return s;
            }
        }
        Debug.LogError("no sound named: " + _name);
        return new SFX();
    }

    private string[] SFXArrToStringArr(SFX[] _arr)
    {
        List<string> _out = new List<string>();
        foreach(SFX _s in _arr)
        {
            _out.Add(_s._name);
        }
        return _out.ToArray();
    }

}
