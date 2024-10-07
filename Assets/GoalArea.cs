using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GoalArea : MonoBehaviour
{
    private int redPlayersInGoal = 0;
    private int bluePlayersInGoal = 0;
    private int yellowPlayersInGoal = 0;

    public TMP_Text overallCounterText;
    public TMP_Text remainingCounterText;
    public TMP_Text treasureCounterText;
    public GameObject winPanel;

    private int totalPlayers = 6;
    private int playersInGoal = 0;
    private int totalTreasures = 3;
    private int treasuresCollected = 0;

    public bool isGameWon = false;

    void Start()
    {
        UpdateUI();
        winPanel.SetActive(false);
    }

    // Handle when players enter the goal area
    private void OnTriggerEnter(Collider other)
    {
        if (isGameWon) return;

        if (other.CompareTag("RedPlayer") && redPlayersInGoal < 2)
        {
            redPlayersInGoal++;
            playersInGoal++;
            UpdateUI();
        }
        else if (other.CompareTag("BluePlayer") && bluePlayersInGoal < 2)
        {
            bluePlayersInGoal++;
            playersInGoal++;
            UpdateUI();
        }
        else if (other.CompareTag("YellowPlayer") && yellowPlayersInGoal < 2)
        {
            yellowPlayersInGoal++;
            playersInGoal++;
            UpdateUI();
        }

        CheckWinCondition();
    }

    // Handle when players leave the goal area
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("RedPlayer") && redPlayersInGoal > 0)
        {
            redPlayersInGoal--;
            playersInGoal--;
            UpdateUI();
        }
        else if (other.CompareTag("BluePlayer") && bluePlayersInGoal > 0)
        {
            bluePlayersInGoal--;
            playersInGoal--;
            UpdateUI();
        }
        else if (other.CompareTag("YellowPlayer") && yellowPlayersInGoal > 0)
        {
            yellowPlayersInGoal--;
            playersInGoal--;
            UpdateUI();
        }
    }

    void UpdateUI()
    {
        overallCounterText.text = playersInGoal + "/" + totalPlayers + " players reached the goal";
        int remaining = totalPlayers - playersInGoal;
        remainingCounterText.text = remaining + " players remaining";
        treasureCounterText.text = "Treasures collected: " + treasuresCollected + "/" + totalTreasures;
    }

    public void CollectTreasure()
    {
        treasuresCollected++;
        UpdateUI();
        CheckWinCondition();
    }

    void CheckWinCondition()
    {
        if (playersInGoal >= totalPlayers && treasuresCollected >= totalTreasures)
        {
            WinLevel();
        }
    }

    void WinLevel()
    {
        winPanel.SetActive(true);
        isGameWon = true;
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
