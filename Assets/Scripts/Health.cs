using System;
using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float deathDelay = 0.5f;

    private int currentHealth;
    private bool isDead = false;

    private Animator animator;
    private Collider2D[] colliders;
    private Enemy enemyScript;
    private Rigidbody2D rb;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;
    public bool IsDead => isDead;

    public event Action<int, int> OnHealthChanged;
    public event Action<Health> OnDamaged;
    public event Action<Health> OnDied;

    private void Awake()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        colliders = GetComponentsInChildren<Collider2D>();
        enemyScript = GetComponent<Enemy>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void TakeDamage(int damage)
    {
        if (damage <= 0 || isDead)
        {
            return;
        }

        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        Debug.Log($"{gameObject.name} HP: {currentHealth}/{maxHealth}");

        OnDamaged?.Invoke(this);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            StartCoroutine(DieRoutine());
        }
    }

    private IEnumerator DieRoutine()
    {
        if (isDead)
        {
            yield break;
        }

        isDead = true;

        foreach (Collider2D col in colliders)
        {
            col.enabled = false;
        }

        if (enemyScript != null)
        {
            enemyScript.enabled = false;
        }

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.simulated = false;
        }

        if (animator != null)
        {
            animator.SetTrigger("die");
        }
        else
        {
            Debug.LogWarning($"{gameObject.name} has no Animator.");
        }

        OnDied?.Invoke(this);

        yield return new WaitForSeconds(deathDelay);

        // Don't destroy player - let the respawn system handle it
        if (CompareTag("Player"))
        {
            gameObject.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ResetHealth()
    {
        // Stop any running coroutines (especially DieRoutine)
        StopAllCoroutines();

        currentHealth = maxHealth;
        isDead = false;

        // Reset animator state
        if (animator != null)
        {
            animator.ResetTrigger("die");
            animator.SetBool("isRunning", false);
            animator.SetBool("isJumping", false);
        }

        // Re-enable colliders
        foreach (Collider2D col in colliders)
        {
            col.enabled = true;
        }

        // Re-enable enemy script if it exists
        if (enemyScript != null)
        {
            enemyScript.enabled = true;
        }

        // Re-enable rigidbody
        if (rb != null)
        {
            rb.simulated = true;
            rb.linearVelocity = Vector2.zero;
        }

        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }
}