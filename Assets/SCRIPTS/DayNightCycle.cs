using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DayNightCycle : MonoBehaviour
{
    [Header("Light Settings")]
    public Light2D globalLight;
    public Color dayColor = Color.white;
    public Color nightColor = new Color(0.30f, 0.35f, 0.65f, 1f); // moonlit blue
    public float dayIntensity = 1.0f;
    public float nightIntensity = 0.30f; // dark, but tiles stay readable

    [Header("Cycle Timing (Seconds)")]
    public float dayDuration = 30f;
    public float nightDuration = 30f;

    [Header("Current State (Read-Only)")]
    public bool isNight = false;
    private float timer = 0f;

    void Start()
    {
        // Find the Global light specifically (not the campfire or other lights)
        if (globalLight == null)
        {
            foreach (var l in FindObjectsByType<Light2D>(FindObjectsSortMode.None))
            {
                if (l.lightType == Light2D.LightType.Global)
                {
                    globalLight = l;
                    break;
                }
            }
        }

        if (globalLight == null)
            Debug.LogWarning("DayNightCycle: No Global Light 2D found. Assign one in the Inspector.");
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (!isNight)
        {
            // Day: stays bright, then fades to night during the last 30% of the day
            float progress = timer / dayDuration;
            float t = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01((progress - 0.7f) / 0.3f));
            ApplyLight(dayColor, nightColor, dayIntensity, nightIntensity, t);

            if (timer >= dayDuration)
            {
                isNight = true;
                timer = 0f;
                Debug.Log("Night has fallen! Shadow enemies are spawning...");
            }
        }
        else
        {
            // Night: stays dark, then fades to day during the last 30% of the night
            float progress = timer / nightDuration;
            float t = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01((progress - 0.7f) / 0.3f));
            ApplyLight(nightColor, dayColor, nightIntensity, dayIntensity, t);

            if (timer >= nightDuration)
            {
                isNight = false;
                timer = 0f;
                Debug.Log("Morning has arrived! The shadows retreat.");

                if (RunManager.Instance != null)
                {
                    RunManager.Instance.AdvanceNight();
                }
            }
        }
    }

    void ApplyLight(Color fromColor, Color toColor, float fromIntensity, float toIntensity, float t)
    {
        if (globalLight == null) return;

        globalLight.color = Color.Lerp(fromColor, toColor, t);
        globalLight.intensity = Mathf.Lerp(fromIntensity, toIntensity, t);
    }
}