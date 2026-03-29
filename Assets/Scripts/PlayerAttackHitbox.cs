using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackHitbox : MonoBehaviour
{
    [SerializeField] private int damage = 1;

    private readonly HashSet<Health> damagedTargets = new HashSet<Health>();
    private EnemyHpUI enemyHpUI;
    private PlayerCombat playerCombat;

    private void Awake()
    {
        enemyHpUI = FindAnyObjectByType<EnemyHpUI>();
        playerCombat = GetComponentInParent<PlayerCombat>();
    }

    public void ResetDamagedTargets()
    {
        damagedTargets.Clear();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy") && !collision.CompareTag("Boss"))
        {
            return;
        }

        // Only deal damage if player is actively attacking
        if (playerCombat == null || !playerCombat.IsAttacking())
        {
            return;
        }

        Health health = collision.GetComponentInParent<Health>();

        if (health == null || damagedTargets.Contains(health) || health.IsDead)
        {
            return;
        }

        damagedTargets.Add(health);
        health.TakeDamage(damage);

        if (enemyHpUI != null)
        {
            string enemyName = collision.CompareTag("Boss") ? "BOSS" : collision.gameObject.name.ToUpper();
            enemyHpUI.ShowEnemy(health, enemyName);
        }
    }
}