using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class HighScoreSaver : MonoBehaviour
{
    public static HighScoreSaver Instance { get;  set; }
    public Leaderboard leaderboard = new Leaderboard();
    
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            
            Instance = this;
        }

        if (!Directory.Exists(Application.persistentDataPath + "/HighScores/"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/HighScores/");
        }
    }

    public void SaveScores(List<HighScoreEntry> scoresToSave)
    {
        if (!Directory.Exists(Application.persistentDataPath + "/HighScores/"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/HighScores/");
        }
        
        if(leaderboard == null)
        {
            leaderboard = new Leaderboard();
        }
        leaderboard.list = scoresToSave;
        XmlSerializer serializer = new XmlSerializer(typeof(Leaderboard));
        FileStream stream = new FileStream(Application.persistentDataPath + "/HighScores/highscores.xml", FileMode.Create);
        serializer.Serialize(stream, leaderboard);
        stream.Close();
    }

    public List<HighScoreEntry> LoadScores()
    {
        if (File.Exists(Application.persistentDataPath + "/HighScores/highscores.xml"))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Leaderboard));
            FileStream stream = new FileStream(Application.persistentDataPath + "/HighScores/highscores.xml", FileMode.Open);
            leaderboard = serializer.Deserialize(stream) as Leaderboard;
        }
        else
        {
            leaderboard = new Leaderboard();
        }

        return leaderboard.list;
    }
}

[System.Serializable]
public class Leaderboard
{
    public List<HighScoreEntry> list = new List<HighScoreEntry>();
}

public class HighScoreEntry
{
    public string playerName;
    public int score;
    public int seed;

    public HighScoreEntry() { }

    public HighScoreEntry(string name, int score, int seed)
    {
        this.playerName = name;
        this.score = score;
        this.seed = seed;
    }
} 
