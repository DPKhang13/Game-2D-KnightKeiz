using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class EnemyHpUI : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI enemyNameText;
    [SerializeField] private float visibleDuration = 1.5f;
    [SerializeField] private float smoothSpeed = 1.5f;

    private Health currentTarget;
    private float targetValue = 1f;
    private Coroutine hideRoutine;
    private Coroutine smoothRoutine;

    private void Awake()
    {
        if (slider != null)
        {
            slider.minValue = 0f;
            slider.maxValue = 1f;
            slider.value = 1f;
        }

        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
        }
    }

    public void ShowEnemy(Health health, string displayName = "ENEMY")
    {
        if (health == null)
        {
            return;
        }

        if (currentTarget != health)
        {
            UnbindCurrentTarget();
            currentTarget = health;
            BindTarget();
        }

        if (enemyNameText != null)
        {
            enemyNameText.text = displayName;
        }

        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
        }

        HandleHealthChanged(health.CurrentHealth, health.MaxHealth);

        if (hideRoutine != null)
        {
            StopCoroutine(hideRoutine);
        }

        hideRoutine = StartCoroutine(HideAfterDelay());
    }

    private void BindTarget()
    {
        if (currentTarget == null)
        {
            return;
        }

        currentTarget.OnHealthChanged += HandleHealthChanged;
        currentTarget.OnDamaged += HandleDamaged;
        currentTarget.OnDied += HandleTargetDied;
    }

    private void UnbindCurrentTarget()
    {
        if (currentTarget == null)
        {
            return;
        }

        currentTarget.OnHealthChanged -= HandleHealthChanged;
        currentTarget.OnDamaged -= HandleDamaged;
        currentTarget.OnDied -= HandleTargetDied;
    }

    private void HandleHealthChanged(int currentHealth, int maxHealth)
    {
        if (maxHealth <= 0)
        {
            targetValue = 0f;
            return;
        }

        targetValue = (float)currentHealth / maxHealth;

        if (smoothRoutine != null)
        {
            StopCoroutine(smoothRoutine);
        }

        smoothRoutine = StartCoroutine(SmoothHpRoutine());
    }

    private void HandleDamaged(Health damagedHealth)
    {
        if (currentTarget != damagedHealth)
        {
            return;
        }

        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
        }

        if (hideRoutine != null)
        {
            StopCoroutine(hideRoutine);
        }

        hideRoutine = StartCoroutine(HideAfterDelay());
    }

    private IEnumerator SmoothHpRoutine()
    {
        while (slider != null && Mathf.Abs(slider.value - targetValue) > 0.005f)
        {
            slider.value = Mathf.MoveTowards(slider.value, targetValue, Time.deltaTime * smoothSpeed);
            yield return null;
        }

        if (slider != null)
        {
            slider.value = targetValue;
        }
    }

    private void HandleTargetDied(Health deadHealth)
    {
        if (currentTarget != deadHealth)
        {
            return;
        }

        targetValue = 0f;

        if (smoothRoutine != null)
        {
            StopCoroutine(smoothRoutine);
        }

        smoothRoutine = StartCoroutine(SmoothHpRoutine());

        if (hideRoutine != null)
        {
            StopCoroutine(hideRoutine);
        }

        hideRoutine = StartCoroutine(HideAfterDelay(0.8f));
    }

    private IEnumerator HideAfterDelay(float delay = -1f)
    {
        if (delay < 0f)
        {
            delay = visibleDuration;
        }

        yield return new WaitForSeconds(delay);

        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
        }

        UnbindCurrentTarget();
        currentTarget = null;
    }
}