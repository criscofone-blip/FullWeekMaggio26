using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;

    [Header("Vision")]
    [SerializeField] private float visionRadius = 15f;
    [SerializeField] private LayerMask obstacleMask;

    [Header("Shooting")]
    [SerializeField] private float fireRate = 1.2f;
    [SerializeField] private float bulletSpeed = 18f;

    [Header("Patrol")]
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float patrolPointReachDistance = 1f;

    private NavMeshAgent agent;
    private int currentPatrolIndex;
    private float nextFireTime;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        GoToNextPatrolPoint();
    }

    private void Update()
    {
        if (CanSeePlayer())
        {
            AttackPlayer();
        }
        else
        {
            Patrol();
        }
    }

    // Controlla se il player è dentro il raggio di visione e se non ci sono ostacoli in mezzo.
    private bool CanSeePlayer()
    {
        if (player == null)
            return false;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > visionRadius)
            return false;

        Vector3 directionToPlayer = (player.position - transform.position).normalized;

        if (Physics.Raycast(transform.position + Vector3.up, directionToPlayer, out RaycastHit hit, visionRadius, obstacleMask))
        {
            return false;
        }

        return true;
    }

    // Ferma il pattugliamento, guarda il player e spara verso la sua posizione esatta in quel momento.
    private void AttackPlayer()
    {
        agent.isStopped = true;

        Vector3 lookDirection = player.position - transform.position;
        lookDirection.y = 0f;

        if (lookDirection.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 12f);
        }

        if (Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    
    // Istanzia un proiettile enemy e lo spara verso la posizione del player.
    private void Shoot()
    {
        if (bulletPrefab == null || firePoint == null || player == null)
            return;

        Vector3 shootDirection =
            (player.position + Vector3.up - firePoint.position).normalized;

        GameObject bulletObject = Instantiate(
            bulletPrefab,
            firePoint.position,
            Quaternion.LookRotation(shootDirection)
        );

        Bullet bullet = bulletObject.GetComponent<Bullet>();

        if (bullet != null)
        {
            bullet.SetDirection(shootDirection);
        }
    }

    // Fa muovere il nemico tra i punti della NavMesh quando non vede il player.
    private void Patrol()
    {
        agent.isStopped = false;

        if (patrolPoints == null || patrolPoints.Length == 0)
            return;

        if (!agent.pathPending && agent.remainingDistance <= patrolPointReachDistance)
        {
            GoToNextPatrolPoint();
        }
    }

    // Manda il nemico al prossimo punto di pattuglia disponibile.
    private void GoToNextPatrolPoint()
    {
        if (patrolPoints == null || patrolPoints.Length == 0)
            return;

        agent.SetDestination(patrolPoints[currentPatrolIndex].position);

        currentPatrolIndex++;
        currentPatrolIndex %= patrolPoints.Length;
    }

    // Disegna il raggio di visione nella Scene View per debug.
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, visionRadius);
    }
}
