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
    private Vector3 checkpointPosition;
    private Vector3 initialSpawnPosition;
    private GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        updateScore();
        gameOverUi.SetActive(false);
        gameWinUi.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            initialSpawnPosition = player.transform.position;
            checkpointPosition = initialSpawnPosition;
        }
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

    public void SetCheckpoint(Vector3 position)
    {
        checkpointPosition = position;
    }

    public void RespawnAtCheckpoint()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if (player == null)
            {
                Debug.LogError("Player not found! Cannot respawn.");
                return;
            }
        }

        Vector3 spawnPosition = checkpointPosition;
        RespawnAtPosition(spawnPosition);
    }



    private void RespawnAtPosition(Vector3 position)
    {
        Time.timeScale = 1;

        if (!player.activeInHierarchy)
        {
            player.SetActive(true);
        }

        player.transform.position = position;

        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.simulated = true;
        }

        Collider2D[] colliders = player.GetComponentsInChildren<Collider2D>();
        foreach (Collider2D col in colliders)
        {
            col.enabled = true;
        }

        Health health = player.GetComponent<Health>();
        if (health != null)
        {
            health.ResetHealth();
        }

        isGameOver = false;
        gameOverUi.SetActive(false);
    }

    public void RestartGame()
    {
        Time.timeScale = 1; // Resume the game
        RespawnAtCheckpoint();
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
