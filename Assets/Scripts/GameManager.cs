using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    private int score = 0;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject gameOverUi;
    [SerializeField] private GameObject gameWinUi;
    [SerializeField] private string checkpointTag = "Checkspawn";
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
            // Always reset checkpoint to initial spawn position on fresh game start
            checkpointPosition = initialSpawnPosition;
            Debug.Log($"[RESPAWN] Game started - Initial spawn position set to: {initialSpawnPosition}");
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
        Debug.Log($"Checkpoint saved at position: {position}");
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

        // Use stored checkpoint position or find nearest one
        Vector3 spawnPosition = checkpointPosition;

        // If we have found checkpoints, use the nearest one
        GameObject[] checkpoints = GameObject.FindGameObjectsWithTag(checkpointTag);
        if (checkpoints.Length > 0)
        {
            spawnPosition = GetNearestCheckpointPosition(player.transform.position);
        }
        else
        {
            // Fallback to stored checkpoint or initial spawn position
            spawnPosition = checkpointPosition;
        }

        Debug.Log($"Respawning at position: {spawnPosition}");
        RespawnAtPosition(spawnPosition);
    }

    public void RespawnAtNearestCheckpoint()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if (player == null)
            {
                return;
            }
        }

        Vector3 nearestCheckpoint = GetNearestCheckpointPosition(player.transform.position);
        checkpointPosition = nearestCheckpoint;
        RespawnAtPosition(nearestCheckpoint);
    }

    private Vector3 GetNearestCheckpointPosition(Vector3 fromPosition)
    {
        Vector3 nearestPosition = checkpointPosition;
        float nearestDistanceSqr = float.MaxValue;
        bool hasCheckpoint = false;

        try
        {
            GameObject[] checkpoints = GameObject.FindGameObjectsWithTag(checkpointTag);
            foreach (GameObject checkpoint in checkpoints)
            {
                if (checkpoint == null)
                {
                    continue;
                }

                float distanceSqr = (checkpoint.transform.position - fromPosition).sqrMagnitude;
                if (distanceSqr < nearestDistanceSqr)
                {
                    nearestDistanceSqr = distanceSqr;
                    nearestPosition = checkpoint.transform.position;
                    hasCheckpoint = true;
                }
            }
        }
        catch (UnityException)
        {
            // Fallback to spawn position if the checkpoint tag does not exist in Tag Manager.
        }

        if (!hasCheckpoint)
        {
            nearestPosition = initialSpawnPosition;
        }

        return nearestPosition;
    }

    private void RespawnAtPosition(Vector3 position)
    {
        Debug.Log($"[RESPAWN] Starting respawn at position: {position}");

        Time.timeScale = 1;

        // Re-enable player if disabled
        if (!player.activeInHierarchy)
        {
            Debug.Log("[RESPAWN] Player was inactive, activating...");
            player.SetActive(true);
        }

        // Reset player position
        Debug.Log($"[RESPAWN] Setting player position to {position}");
        player.transform.position = position;

        // Reset rigidbody
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.simulated = true;
        }

        // Reset colliders
        Collider2D[] colliders = player.GetComponentsInChildren<Collider2D>();
        foreach (Collider2D col in colliders)
        {
            col.enabled = true;
        }

        // Reset health
        Health health = player.GetComponent<Health>();
        if (health != null)
        {
            health.ResetHealth();
            Debug.Log("[RESPAWN] Health reset");
        }

        isGameOver = false;
        gameOverUi.SetActive(false);

        Debug.Log($"[RESPAWN] Respawn complete. Player position: {player.transform.position}");
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
