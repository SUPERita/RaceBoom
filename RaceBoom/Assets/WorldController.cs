using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WorldController : MonoBehaviour
{
    [InlineEditor]
    [SerializeField] private WorldData[] worldDatas = null;
    [SerializeField] private Slider progressSlider = null;
    [SerializeField] private TextMeshProUGUI worldNumberText = null;
    [SerializeField] private TextMeshProUGUI mainMenuWorldNumberText = null;
    [ShowInInspector]
    [ReadOnly]
    private int currentWorld = 0;
    [ShowInInspector]
    [ReadOnly]
    private int positionWhenEnteredLastWorld = 0;
    private WorldData CurrentWorld()
    {
        return worldDatas[currentWorld];
    }

    private const string currentWorldSaveLoc = "crntWorld";
    [SerializeField] private HighscoreDisplay score = null;
    [SerializeField] private GameEventManager gameEventManager = null;

    private void OnDisable()
    {
        gameEventManager.OnGameOver -= GameEventManager_OnGameOver;
        gameEventManager.OnGameStart -= GameEventManager_OnGameStart;
    }
    private void Start()
    {
        gameEventManager.OnGameOver += GameEventManager_OnGameOver;
        gameEventManager.OnGameStart += GameEventManager_OnGameStart;

        SetViewEnabaled(false);
    }

    private void GameEventManager_OnGameStart()
    {
        SetViewEnabaled(true);
    }

    private void GameEventManager_OnGameOver()
    {
        SetViewEnabaled(false);
    }
    private void SetViewEnabaled(bool _arg)
    {
        worldNumberText.gameObject.SetActive(_arg);
        progressSlider.gameObject.SetActive(_arg);
        mainMenuWorldNumberText.gameObject.SetActive(!_arg);

    }


    private void Awake()
    {
        currentWorld = GetCurrentWorldSave();
    }

    public GameObject GetRandomPartFromCurrentWorld()
    {
        return CurrentWorld().GetRandomPart();
    }


    private void Update()
    {
        //visuals
        progressSlider.value = Mathf.Lerp(
            progressSlider.value,
            (float)(score.tmpScore - positionWhenEnteredLastWorld /*- CurrentWorld().worldStart*/) / (float)(CurrentWorld().worldEnd - CurrentWorld().worldStart),
            .9f);
        worldNumberText.SetText((currentWorld+1).ToString());
        mainMenuWorldNumberText.SetText((currentWorld + 1).ToString());

        if (GetCurrentPlayerObjectivePosition() > CurrentWorld().worldEnd + positionWhenEnteredLastWorld)
        {
            //Debug.Log(GetCurrentPlayerObjectivePosition() + "         " + CurrentWorld().worldEnd);
            //Debug.Log(score.tmpScore);
            PassWorld();
            positionWhenEnteredLastWorld = score.tmpScore;
        }
    }

    private int GetCurrentPlayerObjectivePosition()
    {
        return score.tmpScore + CurrentWorld().worldStart;
    }
    private void PassWorld()
    {
        Debug.Log("next");
        currentWorld++;
        SetCurrentWorldSave(currentWorld);
    }



    [Button]
    private void Editor_ResetCurrentWorldSave()
    {
        SaveSystem.SaveIntAtLocation(0, currentWorldSaveLoc);
    }
    private void SetCurrentWorldSave(int _i)
    {
        SaveSystem.SaveIntAtLocation(_i, currentWorldSaveLoc);
    }
    private int GetCurrentWorldSave()
    {
        return SaveSystem.LoadIntFromLocation(currentWorldSaveLoc);
    }

    [Button]
    private void ValidateWorldStartsEnds()
    {
        bool flag = false;
        for (int i = 0; i < worldDatas.Length; i++)
        {
            //if it is the first world
            if(i == 0)
            {
                if(worldDatas[i].worldStart == 0){continue;}
                else{ Debug.LogError("problem with first world start location"); flag = true; }
            } 
            //check back
            if(worldDatas[i-1].worldEnd != worldDatas[i].worldStart)
            {
                flag = true;
                Debug.LogError("problem with world: "+ (i+1));
            }

            //check self
            if (worldDatas[i].worldEnd < worldDatas[i].worldStart)
            {
                flag = true;
                Debug.LogError("problem with world: " + (i + 1));
            }
        }
        if (!flag) { Debug.Log("ALL GOOD"); }
    }

}
