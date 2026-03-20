using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private GameManager gameManager;
    private AudioManager audioManager;
    private void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        audioManager = FindAnyObjectByType<AudioManager>();

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin"))
        {
            Destroy(collision.gameObject);
            gameManager.AddScore(1);
            audioManager.PlayCoinSound();
        }
        else if (collision.CompareTag("Trap"))
        {
            gameManager.GameOver();
            // Implement game over logic here (e.g., restart level, show game over screen)
        }
        else if (collision.CompareTag("Enemy"))
        {
            gameManager.GameOver();
            // Implement game over logic here (e.g., restart level, show game over screen)
        }
        else if (collision.CompareTag("Key"))
        {
            Destroy(collision.gameObject);
            gameManager.GameWin();
        }
    }
}
