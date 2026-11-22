using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VolumeSettings : MonoBehaviour
{
    [Header("UI References")]
    public Slider volumeSlider;
    public TMP_Text percentageText; // Optional: drag a percentage text label here

    [Header("Settings")]
    public string saveKey = "MasterVolume";
    public float defaultVolume = 1f;

    void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat(saveKey, defaultVolume);
        ApplyVolume(savedVolume);

        if (volumeSlider != null)
        {
            volumeSlider.SetValueWithoutNotify(savedVolume);
            volumeSlider.onValueChanged.AddListener(OnSliderChanged);
        }
    }

    public void OnSliderChanged(float value)
    {
        ApplyVolume(value);
        PlayerPrefs.SetFloat(saveKey, value);
    }

    private void ApplyVolume(float value)
    {
        float clamped = Mathf.Clamp01(value);
        AudioListener.volume = clamped;

        if (percentageText != null)
        {
            percentageText.text = Mathf.RoundToInt(clamped * 100f) + "%";
        }
    }
}