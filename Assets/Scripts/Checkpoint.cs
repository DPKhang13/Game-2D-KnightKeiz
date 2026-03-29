using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private GameManager gameManager;

    private void Start()
    {
        // Ensure the collider is set as trigger
        Collider2D col = GetComponent<Collider2D>();
        if (col != null && !col.isTrigger)
        {
            Debug.LogWarning($"Checkpoint {gameObject.name} collider is not set as trigger!");
        }

        gameManager = FindAnyObjectByType<GameManager>();
        Debug.Log($"Checkpoint {gameObject.name} initialized at position {transform.position}");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (gameManager == null)
            {
                gameManager = FindAnyObjectByType<GameManager>();
            }

            if (gameManager != null)
            {
                Debug.Log($"Checkpoint reached at position {transform.position}");
                gameManager.SetCheckpoint(transform.position);
            }
        }
    }
}
