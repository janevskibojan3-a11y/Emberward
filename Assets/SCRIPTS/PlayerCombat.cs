using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerCombat : MonoBehaviour
{
    [Header("Combat Settings")]
    public Transform attackPoint;
    public GameObject slashPrefab; // Drag your slash visual prefab here
    public float attackRange = 0.8f;
    public int attackDamage = 1;
    public float attackCooldown = 0.4f;
    public LayerMask enemyLayers;

    private float nextAttackTime = 0f;

    void Update()
    {
        AimAttackPointAtMouse();

        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        if (Time.time >= nextAttackTime)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Attack();
                nextAttackTime = Time.time + attackCooldown;
            }
        }
    }

    void AimAttackPointAtMouse()
    {
        if (attackPoint == null || Camera.main == null) return;

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        Vector2 direction = (mouseWorldPos - transform.position).normalized;
        attackPoint.localPosition = direction * 0.7f;

        // Rotate AttackPoint to face the mouse cursor
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        attackPoint.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    void Attack()
    {
        Debug.Log("Soldier swung sword!");

        // Spawn the visual slash at the attack point's position and rotation
        if (slashPrefab != null && attackPoint != null)
        {
            Instantiate(slashPrefab, attackPoint.position, attackPoint.rotation);
        }

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(attackDamage);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}