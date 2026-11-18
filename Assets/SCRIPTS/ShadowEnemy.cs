using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(EnemyHealth))]
public class ShadowEnemy : MonoBehaviour
{
    [Header("Movement & Combat")]
    public float moveSpeed = 2.2f;
    public int contactDamage = 15;
    public float attackCooldown = 1.0f;

    [Header("Light Weakness (Optional)")]
    public Transform campfire;
    public float lightSlowRadius = 4.0f;
    public float lightSpeedMultiplier = 0.5f;

    private Transform player;
    private Rigidbody2D rb;
    private float nextDamageTime = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;

        // Automatically find your soldier and campfire if not assigned
        GameObject playerObj = GameObject.Find("Character1_Idle");
        if (playerObj != null) player = playerObj.transform;

        if (campfire == null)
        {
            GameObject fireObj = GameObject.Find("CampfireLight");
            if (fireObj != null) campfire = fireObj.transform;
        }
    }

    void FixedUpdate()
    {
        if (player == null) return;

        Vector2 direction = ((Vector2)player.position - rb.position).normalized;
        float currentSpeed = moveSpeed;

        // Slow down if the shadow creature enters the campfire light
        if (campfire != null && Vector2.Distance(rb.position, campfire.position) <= lightSlowRadius)
        {
            currentSpeed *= lightSpeedMultiplier;
        }

        rb.MovePosition(rb.position + direction * currentSpeed * Time.fixedDeltaTime);
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Character1_Idle" && Time.time >= nextDamageTime)
        {
            // Uses SendMessage so it works regardless of your exact PlayerHealth method name
            collision.gameObject.SendMessage("TakeDamage", contactDamage, SendMessageOptions.DontRequireReceiver);
            nextDamageTime = Time.time + attackCooldown;
            Debug.Log("Shadow enemy hit the soldier!");
        }
    }
}