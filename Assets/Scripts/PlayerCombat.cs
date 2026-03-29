using System.Collections;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Collider2D attackHitbox;
    [SerializeField] private float attackDuration = 0.12f;
    [SerializeField] private float attackCooldown = 0.3f;

    private bool canAttack = true;
    private bool isAttackingNow = false;
    private PlayerAttackHitbox attackHitboxScript;
    private PlayerStats playerStats;
    private void Awake()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();

        }

        if (attackHitbox != null)
        {
            attackHitbox.enabled = false;
            playerStats = GetComponent<PlayerStats>();
            attackHitboxScript = attackHitbox.GetComponent<PlayerAttackHitbox>();
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && canAttack)
        {

            if (playerStats != null && playerStats.UseStamina(15f))
            {
                StartCoroutine(AttackRoutine());
            }
        }
    }

    private IEnumerator AttackRoutine()
    {
        canAttack = false;
        isAttackingNow = true;

        if (animator != null)
        {
            animator.SetTrigger("isAttacking");
        }

        yield return new WaitForSeconds(0.05f);

        if (attackHitboxScript != null)
        {
            attackHitboxScript.ResetDamagedTargets();
        }

        if (attackHitbox != null)
        {
            attackHitbox.enabled = true;
        }

        yield return new WaitForSeconds(attackDuration);

        if (attackHitbox != null)
        {
            attackHitbox.enabled = false;
        }

        isAttackingNow = false;

        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    public bool IsAttacking()
    {
        return isAttackingNow;
    }
}