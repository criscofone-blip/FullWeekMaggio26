using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform target;
    [SerializeField] private InputActionReference lookAction;

    [Header("Camera")]
    [SerializeField] private Vector3 offset = new Vector3(0f, 2.2f, -5f);
    [SerializeField] private float mouseSensitivity = 0.1f;
    [SerializeField] private float followSpeed = 12f;

    [Header("Pitch")]
    [SerializeField] private float minPitch = -35f;
    [SerializeField] private float maxPitch = 65f;

    private float yaw;
    private float pitch;

    private void OnEnable()
    {
        lookAction.action.Enable();
    }

    private void OnDisable()
    {
        lookAction.action.Disable();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        yaw = target.eulerAngles.y;
    }

    private void LateUpdate()
    {
        HandleCamera();
    }

    private void HandleCamera()
    {
        Vector2 lookInput = lookAction.action.ReadValue<Vector2>();

        yaw += lookInput.x * mouseSensitivity;
        pitch -= lookInput.y * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        Quaternion cameraRotation = Quaternion.Euler(pitch, yaw, 0f);

        Vector3 desiredPosition = target.position + cameraRotation * offset;

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            followSpeed * Time.deltaTime
        );

        transform.LookAt(target.position + Vector3.up * 1.5f);
    }
}