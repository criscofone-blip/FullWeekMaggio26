using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip shooting;

    [Header("References")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private GameObject playerBulletPrefab;

    [Header("Input")]
    [SerializeField] private InputActionReference shootAction;

    [Header("Shooting")]
    [SerializeField] private float fireRate = 0.25f;
    [SerializeField] private float spawnDistanceFromCamera = 0.5f;

    private float nextFireTime;

    private void Awake()
    {
        if (playerCamera == null)
            playerCamera = Camera.main;
    }

    private void OnEnable()
    {
        shootAction.action.Enable();
    }

    private void OnDisable()
    {
        shootAction.action.Disable();
    }

    private void Update()
    {
        HandleShoot();
    }

    // Controlla l'input di sparo del player.
    private void HandleShoot()
    {
        if (!shootAction.action.IsPressed())
            return;

        if (Time.time < nextFireTime)
            return;

        Shoot();

        nextFireTime = Time.time + fireRate;
    }

    // Spara dal centro della camera verso la direzione della camera.
    private void Shoot()
    {
        audioSource.PlayOneShot(shooting);
        Vector3 spawnPosition =
            playerCamera.transform.position +
            playerCamera.transform.forward * spawnDistanceFromCamera;

        GameObject bulletObject = Instantiate(
            playerBulletPrefab,
            spawnPosition,
            Quaternion.LookRotation(playerCamera.transform.forward)
        );

        Bullet bullet = bulletObject.GetComponent<Bullet>();

        if (bullet != null)
        {
            bullet.SetDirection(playerCamera.transform.forward);
        }
    }
}