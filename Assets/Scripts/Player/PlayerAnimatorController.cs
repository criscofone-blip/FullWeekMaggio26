using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimationController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;

    [Header("Input")]
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference shootAction;

    [Header("Animator Parameters")]
    [SerializeField] private string walkParameter = "IsWalking";
    [SerializeField] private string shootParameter = "IsShooting";

    private void Awake()
    {
        if (animator == null)
            animator = GetComponentInChildren<Animator>();
    }

    private void OnEnable()
    {
        moveAction.action.Enable();
        shootAction.action.Enable();
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
        shootAction.action.Disable();
    }

    private void Update()
    {
        HandleMovementAnimation();
        HandleShootAnimation();
    }

    // Aggiorna la bool di camminata.
    private void HandleMovementAnimation()
    {
        Vector2 moveInput = moveAction.action.ReadValue<Vector2>();

        bool isWalking = moveInput.sqrMagnitude > 0.01f;

        animator.SetBool(walkParameter, isWalking);
    }

    // Aggiorna la bool di sparo.
    private void HandleShootAnimation()
    {
        bool isShooting = shootAction.action.IsPressed();

        animator.SetBool(shootParameter, isShooting);
    }
}
