using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip pickedUp;

    [Header("Input")]
    [SerializeField] private InputActionReference interactAction;

    [Header("Detection")]
    [SerializeField] private float interactRadius = 2f;
    [SerializeField] private LayerMask collectibleLayer;

    private CollectiblePiece currentCollectible;

    private void OnEnable()
    {
        interactAction.action.Enable();
    }

    private void OnDisable()
    {
        interactAction.action.Disable();
    }

    private void Update()
    {
        DetectCollectible();
        HandleInteraction();
    }

    // Cerca oggetti raccoglibili vicino al player.
    private void DetectCollectible()
    {
        currentCollectible = null;

        Collider[] colliders = Physics.OverlapSphere(
            transform.position,
            interactRadius,
            collectibleLayer
        );

        foreach (Collider collider in colliders)
        {
            CollectiblePiece collectible =
                collider.GetComponent<CollectiblePiece>();

            if (collectible != null)
            {
                currentCollectible = collectible;
                return;
            }
        }
    }

    // Controlla se il player preme il tasto di interazione.
    private void HandleInteraction()
    {
        if (currentCollectible == null)
            return;

        if (interactAction.action.WasPressedThisFrame())
        {
            audioSource.PlayOneShot(pickedUp);
            currentCollectible.Collect();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(
            transform.position,
            interactRadius
        );
    }
}