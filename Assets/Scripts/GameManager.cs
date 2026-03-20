using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    private int score = 0;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject gameOverUi;
    [SerializeField] private GameObject gameWinUi;
    private bool isGameOver = false;
    private bool isGameWin = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        updateScore();
        gameOverUi.SetActive(false);
        gameWinUi.SetActive(false);
    }

    public void AddScore(int points)
    {
        if (!isGameOver && !isGameWin)
        {
            score += points;
            updateScore();
        }
    }
    private void updateScore()
    {
        scoreText.text = score.ToString();
    }
    public void GameOver()
    {
        if (!isGameOver)
        {
            isGameOver = true;
            score = 0;
            Time.timeScale = 0; // Pause the game
            gameOverUi.SetActive(true);
            // Implement additional game over logic here (e.g., stop player movement, show final score)
        }
    }
    public void GameWin()
    {
        if (!isGameWin)
        {
            isGameWin = true;
            Time.timeScale = 0; // Pause the game
            gameWinUi.SetActive(true);
            // Implement additional game win logic here (e.g., stop player movement, show final score)
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1; // Resume the game
        score = 0;
        updateScore();
        gameOverUi.SetActive(false);
        isGameOver = false;
        SceneManager.LoadScene("Game"); // Reload the current scene
        // Implement additional restart logic here (e.g., reset player position, respawn coins)
    }
    public bool IsGameOver()
    {
        return isGameOver;
    }
    public bool IsGameWin()
    {
        return isGameWin;
    }
}
