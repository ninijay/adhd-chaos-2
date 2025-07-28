using System;
using System.Collections.Generic;
using UnityEngine;

public class HighScores : MonoBehaviour
{
    public HighScoreDisplay[] highScoreDisplayArray;
    List<HighScoreEntry> scores = new List<HighScoreEntry>();
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (HighScoreSaver.Instance == null)
        {
            HighScoreSaver.Instance = new HighScoreSaver();
        }
        scores = HighScoreSaver.Instance.LoadScores();
        if(scores == null)
        {
            scores = new List<HighScoreEntry>();
        }
        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        scores.Sort((HighScoreEntry x, HighScoreEntry y) => y.score.CompareTo(x.score));

        for (int i = 0; i < highScoreDisplayArray.Length; i++)
        {
            if (i < scores.Count)
            {
                highScoreDisplayArray[i].DisplayHighScore(scores[i].playerName, scores[i].score, scores[i].seed);
            }
            else
            {
                highScoreDisplayArray[i].HideEntryDisplay();
            }
        }
    }

    // Update is called once per frame
    public void AddNewScore(string playerName, int score, int seed)
    {
        HighScoreEntry newEntry = new HighScoreEntry(playerName, score, seed);
        scores.Add(newEntry);
    }
    
    public void SaveScores()
    {
        if (HighScoreSaver.Instance == null)
        {
            HighScoreSaver.Instance = new HighScoreSaver();
        }
        HighScoreSaver.Instance.SaveScores(scores);
    }
}
