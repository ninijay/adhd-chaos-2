using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] public float timeRemainingInSeconds = 120;
    public bool timerIsRunning { get; private set; } = false;
    private TMP_Text timeText;

    private void Start()
    {
        // Starts the timer automatically
        timerIsRunning = true;
        timeText = GetComponent<TMP_Text>();
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemainingInSeconds > 0)
            {
                timeRemainingInSeconds -= Time.deltaTime;
                DisplayTime(timeRemainingInSeconds);
            }
            else
            {
                Debug.Log("Time has run out!");
                timeRemainingInSeconds = 0;
                timerIsRunning = false;
            }
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60); 
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
