using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class ThirdPersonController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform cameraTransform;

    [Header("Input")]
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference lookAction;
    [SerializeField] private InputActionReference jumpAction;
    [SerializeField] private InputActionReference sprintAction;

    [Header("Movement")]
    [SerializeField] private float walkSpeed = 4.5f;
    [SerializeField] private float sprintSpeed = 7f;
    [SerializeField] private float rotationSpeed = 12f;
    [SerializeField] private float jumpHeight = 1.2f;
    [SerializeField] private float gravity = -20f;

    [Header("Ground Check")]
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private float groundCheckOffset = 0.05f;
    [SerializeField] private LayerMask groundMask = ~0;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isOnTheFloor;

    public Vector2 MoveInput { get; private set; }
    public bool IsOnTheFloor => isOnTheFloor;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;
    }

    private void OnEnable()
    {
        moveAction.action.Enable();
        lookAction.action.Enable();
        jumpAction.action.Enable();

        if (sprintAction != null)
            sprintAction.action.Enable();
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
        lookAction.action.Disable();
        jumpAction.action.Disable();

        if (sprintAction != null)
            sprintAction.action.Disable();
    }

    private void Update()
    {
        CheckIfOnFloor();
        HandleMovement();
    }

    private void CheckIfOnFloor()
    {
        Vector3 bottom = transform.position
                       + controller.center
                       - Vector3.up * (controller.height * 0.5f);

        Vector3 checkPosition = bottom + Vector3.down * groundCheckOffset;

        isOnTheFloor = Physics.CheckSphere(
            checkPosition,
            groundCheckRadius,
            groundMask,
            QueryTriggerInteraction.Ignore
        );
    }

    private void HandleMovement()
    {
        MoveInput = moveAction.action.ReadValue<Vector2>();

        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        camForward.y = 0f;
        camRight.y = 0f;

        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDirection =
            camForward * MoveInput.y +
            camRight * MoveInput.x;

        bool isSprinting =
            sprintAction != null &&
            sprintAction.action.IsPressed();

        float currentSpeed = isSprinting ? sprintSpeed : walkSpeed;

        controller.Move(moveDirection * currentSpeed * Time.deltaTime);

        if (moveDirection.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }

        if (isOnTheFloor && velocity.y < 0f)
            velocity.y = -2f;

        if (jumpAction.action.WasPressedThisFrame() && isOnTheFloor)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    private void OnDrawGizmosSelected()
    {
        if (!controller)
            controller = GetComponent<CharacterController>();

        Vector3 bottom = transform.position
                       + controller.center
                       - Vector3.up * (controller.height * 0.5f);

        Vector3 checkPosition = bottom + Vector3.down * groundCheckOffset;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(checkPosition, groundCheckRadius);
    }
}