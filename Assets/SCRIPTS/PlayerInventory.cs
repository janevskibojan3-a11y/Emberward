using UnityEngine;
using TMPro; // Required for TextMeshPro UI

public class PlayerInventory : MonoBehaviour
{
    public int woodCount = 0;

    [Header("UI Reference")]
    public TMP_Text woodCountText;

    void Start()
    {
        UpdateWoodUI();
    }

    public void AddWood(int amount)
    {
        woodCount += amount;
        Debug.Log($"Wood collected! Total wood: {woodCount}");
        UpdateWoodUI();
    }

    public bool SpendWood(int amount)
    {
        if (woodCount >= amount)
        {
            woodCount -= amount;
            Debug.Log($"Used {amount} wood. Remaining: {woodCount}");
            UpdateWoodUI();
            return true;
        }
        Debug.Log("Not enough wood! Go chop a tree.");
        return false;
    }

    private void UpdateWoodUI()
    {
        if (woodCountText != null)
        {
            woodCountText.text = woodCount.ToString();
        }
    }
}