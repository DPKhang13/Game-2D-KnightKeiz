using System;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private int maxHealth = 5;
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float staminaRegenRate = 20f;

    private int currentHealth;
    private float currentStamina;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;
    public float CurrentStamina => currentStamina;
    public float MaxStamina => maxStamina;

    public event Action<int, int> OnHealthChanged;
    public event Action<float, float> OnStaminaChanged;

    private void Awake()
    {
        currentHealth = maxHealth;
        currentStamina = maxStamina;
    }

    private void Start()
    {
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        OnStaminaChanged?.Invoke(currentStamina, maxStamina);
    }

    private void Update()
    {
        if (currentStamina < maxStamina)
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
            currentStamina = Mathf.Min(currentStamina, maxStamina);
            OnStaminaChanged?.Invoke(currentStamina, maxStamina);
        }
    }

    public void TakeDamage(int damage)
    {
        if (damage <= 0) return;

        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public bool UseStamina(float amount)
    {
        if (amount <= 0f) return true;
        if (currentStamina < amount) return false;

        currentStamina -= amount;
        currentStamina = Mathf.Max(currentStamina, 0f);
        OnStaminaChanged?.Invoke(currentStamina, maxStamina);
        return true;
    }
}