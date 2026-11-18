using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public float lifetime = 0.15f; // How long the slash stays visible

    void Start()
    {
        Destroy(gameObject, lifetime);
    }
}