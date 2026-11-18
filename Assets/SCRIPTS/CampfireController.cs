using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

[RequireComponent(typeof(Light2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class CampfireController : MonoBehaviour
{
    [Header("Fuel Settings")]
    public float maxFuel = 100f;
    public float currentFuel = 60f;
    public float fuelDrainPerSecond = 2.5f;
    public float fuelPerWood = 20f;

    [Header("UI Reference")]
    public Slider campfireSlider;

    [Header("Safe Light Radius")]
    public float minRadius = 1.5f;
    public float maxRadius = 7.5f;
    public float flickerAmount = 0.15f;
    public float flickerSpeed = 8f;

    [Header("Damage Settings")]
    public float burnDamagePerSecond = 5f;

    private Light2D fireLight;
    private CircleCollider2D safeZoneTrigger;

    public float CurrentRadius { get; private set; }
    public bool IsExtinguished => currentFuel <= 0f;

    void Awake()
    {
        fireLight = GetComponent<Light2D>();
        safeZoneTrigger = GetComponent<CircleCollider2D>();
        safeZoneTrigger.isTrigger = true;
    }

    void Start()
    {
        if (campfireSlider != null)
        {
            campfireSlider.maxValue = maxFuel;
            campfireSlider.value = currentFuel;
        }
    }

    void Update()
    {
        if (currentFuel > 0f)
        {
            currentFuel = Mathf.Max(0f, currentFuel - fuelDrainPerSecond * Time.deltaTime);
        }

        if (campfireSlider != null)
        {
            campfireSlider.value = currentFuel;
        }

        float fuelPercent = currentFuel / maxFuel;
        float baseRadius = Mathf.Lerp(minRadius, maxRadius, fuelPercent);

        float flicker = currentFuel > 0f 
            ? (Mathf.PerlinNoise(Time.time * flickerSpeed, 0f) - 0.5f) * 2f * flickerAmount 
            : 0f;

        CurrentRadius = Mathf.Max(0.5f, baseRadius + flicker);

        if (fireLight != null)
        {
            fireLight.pointLightOuterRadius = CurrentRadius;
            fireLight.pointLightInnerRadius = CurrentRadius * 0.25f;
        }
        
        if (safeZoneTrigger != null)
        {
            safeZoneTrigger.radius = baseRadius;
        }
    }

    public void AddWood(int amount = 1)
    {
        currentFuel = Mathf.Clamp(currentFuel + (fuelPerWood * amount), 0f, maxFuel);
        
        if (campfireSlider != null)
        {
            campfireSlider.value = currentFuel;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        // Only burn enemies inside the light ring when the campfire is lit
        if (!IsExtinguished && other.CompareTag("Enemy"))
        {
            other.SendMessage("TakeDamage", burnDamagePerSecond * Time.deltaTime, SendMessageOptions.DontRequireReceiver);
        }
    }
}