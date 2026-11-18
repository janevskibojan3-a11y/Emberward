using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Interaction Range")]
    public float interactionRange = 1.5f;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer spriteRenderer;
    private PlayerInventory inventory;
    private Vector2 movementInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        inventory = GetComponent<PlayerInventory>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        // 1. Gather WASD / Arrow Key input
        movementInput.x = Input.GetAxisRaw("Horizontal");
        movementInput.y = Input.GetAxisRaw("Vertical");
        movementInput = movementInput.normalized;

        // 2. Flip sprite left or right based on horizontal movement
        if (movementInput.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (movementInput.x < 0)
        {
            spriteRenderer.flipX = true;
        }

        // 3. Trigger Walk vs Idle animation
        if (anim != null)
        {
            bool isMoving = movementInput.sqrMagnitude > 0.01f;
            anim.SetBool("isWalking", isMoving);
        }

        // 4. Interact with Campfire or Trees
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space))
        {
            TryInteract();
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = movementInput * moveSpeed;
    }

    void TryInteract()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, interactionRange);

        foreach (var hit in hits)
        {
            // 1. Check if near Campfire
            CampfireController campfire = hit.GetComponent<CampfireController>();
            if (campfire != null)
            {
                if (inventory != null && inventory.SpendWood(1))
                {
                    campfire.AddWood(1);
                    Debug.Log("Fed 1 wood to the campfire!");
                }
                return;
            }

            // 2. Check if near a Tree
            ResourceNode tree = hit.GetComponent<ResourceNode>();
            if (tree != null)
            {
                tree.Chop(inventory);
                return;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}