using UnityEngine;

public class CollectiblePiece : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int value = 1;

    private bool collected;

    // Quando il player entra nel trigger, raccoglie il pezzo.
    private void OnTriggerEnter(Collider other)
    {
        if (collected)
            return;

        PlayerHealth player = other.GetComponentInParent<PlayerHealth>();

        if (player == null)
            return;

        collected = true;

        GameManager.Instance.AddPiece(value);

        Destroy(gameObject);
    }
}