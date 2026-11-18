using UnityEngine;

public class ResourceNode : MonoBehaviour
{
    public int hitsRequired = 3;
    public int woodYield = 2;

    private int currentHits = 0;

    public void Chop(PlayerInventory inventory)
    {
        currentHits++;
        Debug.Log($"Chopped {gameObject.name}: {currentHits}/{hitsRequired}");

        if (currentHits >= hitsRequired)
        {
            if (inventory != null)
            {
                inventory.AddWood(woodYield);
            }
            gameObject.SetActive(false); // Tree disappears once harvested
        }
    }
}