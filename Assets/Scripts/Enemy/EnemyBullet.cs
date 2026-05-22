using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [Header("Bullet")]
    [SerializeField] private float lifeTime = 5f;
    [SerializeField] private LayerMask hitMask;

    private Vector3 direction;
    private float speed;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        MoveBullet();
    }

    // Imposta direzione e velocità del proiettile quando viene sparato.
    public void SetDirection(Vector3 newDirection, float newSpeed)
    {
        direction = newDirection.normalized;
        speed = newSpeed;
    }

    // Muove il proiettile nello spazio 3D usando una sprite 2D come visuale.
    private void MoveBullet()
    {
        float moveDistance = speed * Time.deltaTime;

        if (Physics.Raycast(transform.position, direction, out RaycastHit hit, moveDistance, hitMask))
        {
            transform.position = hit.point;
            Destroy(gameObject);
            return;
        }

        transform.position += direction * moveDistance;
    }
}