using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreDisplay : MonoBehaviour
{
    private string highscoreSaveLoc = "highscoreLoc";
    [SerializeField] private string highscoreDisplaySalt = "HighScore: ";
    [SerializeField] private string scoreDisplaySalt = "Score: ";
    [SerializeField] private TextMeshProUGUI scoreDisplay = null;
    [SerializeField] private TextMeshProUGUI highscoreDisplay = null;
    [SerializeField] private GameEventManager gameEventManager = null;

    public int tmpScore { get; private set; } = 0;
    private int tmpHighScore = 0;
    private bool highscoreCanBeBroken = true;
    private void Start()
    {
        gameEventManager.OnGameStart += GameEventManager_OnGameStart;
        gameEventManager.OnGameOver += GameEventManager_OnGameOver;

        SetScoreEnabled(false);

        tmpHighScore = GetHighScore();
        UpdateHighscore(tmpHighScore);
    }

    private void OnDisable()
    {
        gameEventManager.OnGameStart -= GameEventManager_OnGameStart;
        gameEventManager.OnGameOver -= GameEventManager_OnGameOver;

        SetHighScore(tmpHighScore);
        //Debug.Log("saved " + tmpHighScore);
    }


    private void GameEventManager_OnGameOver()
    {
        SetScoreEnabled(false);
    }
    private void GameEventManager_OnGameStart()
    {
        SetScoreEnabled(true);

        highscoreCanBeBroken = true;
    }
    private void SetScoreEnabled(bool _arg)
    {
        scoreDisplay.enabled = _arg;
        scoreDisplay.GetComponentInParent<Image>().enabled = _arg;
    }

    public void UpdateDisplay(float _score)
    {
        //check if broke a record
        if(highscoreCanBeBroken && _score > tmpHighScore)
        {
            MessageManager.instance.SendMessage("RECORD BROKE!!!");
            SoundPool.instance.PlaySound("broke_record");
            highscoreCanBeBroken = false;
        }

        tmpScore = (int)_score;
        if (tmpScore > tmpHighScore) tmpHighScore = tmpScore;
        //tmpHighScore = tmpScore > tmpHighScore ? tmpScore : tmpHighScore;
        

        scoreDisplay.SetText(scoreDisplaySalt + ResourceManager.AddCommas((int)tmpScore));
        highscoreDisplay.SetText(highscoreDisplaySalt + ResourceManager.AddCommas((int)tmpHighScore));
        

    }
    private void UpdateHighscore(float _score)
    {
        if (!highscoreDisplay) return;
        highscoreDisplay.SetText(highscoreDisplaySalt + ResourceManager.AddCommas((int)_score));
    }

    private int GetHighScore()
    {
        return SaveSystem.LoadIntFromLocation(highscoreSaveLoc, 0);
    }
    private void SetHighScore(int _a)
    {
        SaveSystem.SaveIntAtLocation(_a, highscoreSaveLoc);
    }

    [Button]
    private void ResetScore()
    {
        tmpHighScore = 0;
        SetHighScore(0);
        UpdateDisplay(0);
    }

    /*
    [SerializeField] private int repetitionPerFrame = 1;
    void Update()
    {
        for (int i = 0; i < repetitionPerFrame; i++)
        {
            SetScore(GetScore());
        }
    }
    */
}
