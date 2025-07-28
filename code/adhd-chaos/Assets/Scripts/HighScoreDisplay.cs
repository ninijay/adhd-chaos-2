using UnityEngine;

public class HighScoreDisplay : MonoBehaviour
{
    public TMPro.TMP_Text highScoreText;
    public TMPro.TMP_Text playerNameText;
    public TMPro.TMP_Text seedText;

    private void Start()
    {
    }

    public void DisplayHighScore(string playerName, int score, int seed)
    {
        highScoreText.text = "" + score;
        playerNameText.text = "" + playerName;
        seedText.text = "" + seed;
    }

    public void HideEntryDisplay()
    {
        highScoreText.text = "";
        playerNameText.text = "";
        seedText.text = "";
    }
}
