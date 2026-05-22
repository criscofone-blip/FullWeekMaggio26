using UnityEngine;

public class CollectiblePiece : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int value = 1;

    private bool collected;

    // Raccoglie il pezzo.
    public void Collect()
    {
        if (collected)
            return;

        collected = true;

        GameManager.Instance.AddPiece(value);

        Destroy(gameObject);
    }
}