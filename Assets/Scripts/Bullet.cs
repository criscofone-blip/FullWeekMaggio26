using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet")]
    [SerializeField] private float speed = 25f;
    [SerializeField] private float lifeTime = 5f;
    [SerializeField] private int damage = 1;

    [Header("Collision")]
    [SerializeField] private LayerMask hitMask;

    private Vector3 direction;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        MoveBullet();
    }

    // Imposta la direzione del colpo.
    public void SetDirection(Vector3 newDirection)
    {
        direction = newDirection.normalized;
    }

    // Muove il proiettile nello spazio 3D e controlla se colpisce qualcosa.
    private void MoveBullet()
    {
        float distance = speed * Time.deltaTime;

        if (Physics.Raycast(transform.position, direction, out RaycastHit hit, distance, hitMask))
        {
            Health health = hit.collider.GetComponentInParent<Health>();

            if (health != null)
            {
                health.TakeDamage(damage);
            }

            Destroy(gameObject);
            return;
        }

        transform.position += direction * distance;
    }
}