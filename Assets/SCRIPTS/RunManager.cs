using UnityEngine;
using TMPro;

public class RunManager : MonoBehaviour
{
    public static RunManager Instance { get; private set; }

    [Header("HUD Reference")]
    public TextMeshProUGUI nightHudText;

    [Header("Run Stats")]
    public int currentNight = 1;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        UpdateNightHUD();
    }

    public void AdvanceNight()
    {
        currentNight++;
        UpdateNightHUD();
    }

    private void UpdateNightHUD()
    {
        if (nightHudText != null)
        {
            nightHudText.text = $"Night {currentNight}";
        }
    }
}