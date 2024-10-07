using TMPro;
using UnityEngine;

public class CountdownTimer : MonoBehaviour
{
    public float timeRemaining = 120;  // Set timer to 2 minutes (120 seconds)
    public TMP_Text timerText;         // Reference to the TextMeshPro text for displaying time
    public GameObject losePanel;       // Reference to the lose panel

    private bool timerIsRunning = true;

    void Start()
    {
        timerIsRunning = true;  // Start the timer when the game begins
        losePanel.SetActive(false);  // Hide the lose panel at the start of the game
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;
                HandleTimeOut();  // Handle what happens when time runs out
            }
        }
    }

    // Function to display the time in a user-friendly format
    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;  // Add 1 second to correct for rounding errors with Time.deltaTime

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);  // Calculate the minutes
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);  // Calculate the seconds

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    // Handle what happens when time runs out (loss condition)
    void HandleTimeOut()
    {
        Debug.Log("Time has run out!");
        losePanel.SetActive(true);  // Show the lose panel when the time runs out
    }
}
