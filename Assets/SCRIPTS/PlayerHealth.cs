using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("UI Reference")]
    public Slider healthSlider;

    [Header("Game Over UI")]
    public GameObject gameOverPanel;

    [Header("Darkness Damage")]
    public float darknessDamagePerSecond = 10f;

    private bool isDead = false;
    private CampfireController campfire;
    private DayNightCycle dayNightCycle;

    void Start()
    {
        currentHealth = maxHealth;
        campfire = FindAnyObjectByType<CampfireController>();
        dayNightCycle = FindAnyObjectByType<DayNightCycle>();

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    void Update()
    {
        if (isDead) return;

        // 1. Daytime = Always 100% safe everywhere
        if (dayNightCycle == null || !dayNightCycle.isNight)
        {
            return;
        }

        // 2. Nighttime = Check if player is inside a LIT campfire's light radius
        bool isSafeInLight = false;
        if (campfire != null && !campfire.IsExtinguished)
        {
            float distanceToFire = Vector2.Distance(transform.position, campfire.transform.position);
            if (distanceToFire <= campfire.CurrentRadius)
            {
                isSafeInLight = true;
            }
        }

        // 3. Nighttime + In the darkness = Take damage
        if (!isSafeInLight)
        {
            TakeDamage(darknessDamagePerSecond * Time.deltaTime);
        }
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth = Mathf.Max(0f, currentHealth - amount);

        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        if (isDead) return;

        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);

        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }
    }

    void Die()
    {
        isDead = true;
        Debug.Log("Player has died!");

        PlayerController controller = GetComponent<PlayerController>();
        if (controller != null)
        {
            controller.enabled = false;
        }

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
        Time.timeScale = 0f;
    }
}