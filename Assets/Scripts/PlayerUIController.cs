using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour
{
    [SerializeField] private Image hpFill;
    [SerializeField] private Image staminaFill;
    [SerializeField] private float smoothSpeed = 8f;

    private PlayerStats playerStats;
    private float hpTarget = 1f;
    private float staminaTarget = 1f;

    private void Awake()
    {
        playerStats = FindAnyObjectByType<PlayerStats>();

        if (hpFill != null)
        {
            hpFill.fillAmount = 1f;
        }

        if (staminaFill != null)
        {
            staminaFill.fillAmount = 1f;
        }
    }

    private void OnEnable()
    {
        if (playerStats != null)
        {
            playerStats.OnHealthChanged += HandleHealthChanged;
            playerStats.OnStaminaChanged += HandleStaminaChanged;
        }
    }

    private void OnDisable()
    {
        if (playerStats != null)
        {
            playerStats.OnHealthChanged -= HandleHealthChanged;
            playerStats.OnStaminaChanged -= HandleStaminaChanged;
        }
    }

    private void Update()
    {
        if (hpFill != null)
        {
            hpFill.fillAmount = Mathf.Lerp(hpFill.fillAmount, hpTarget, Time.deltaTime * smoothSpeed);
        }

        if (staminaFill != null)
        {
            staminaFill.fillAmount = Mathf.Lerp(staminaFill.fillAmount, staminaTarget, Time.deltaTime * smoothSpeed);
        }
    }

    private void HandleHealthChanged(int current, int max)
    {
        hpTarget = max > 0 ? (float)current / max : 0f;
    }

    private void HandleStaminaChanged(float current, float max)
    {
        staminaTarget = max > 0 ? current / max : 0f;
    }
}