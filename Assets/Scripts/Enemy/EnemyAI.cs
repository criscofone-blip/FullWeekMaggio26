using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] versiMumble ;
    public float soundDelay = 10f;
    private float soundTimer;

    [Header("References")]   
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;

    [Header("Vision")]
    [SerializeField] private float visionRadius = 15f;
    [SerializeField] private LayerMask obstacleMask;

    [Header("Shooting")]
    [SerializeField] private float fireRate = 1.2f;

    [Header("Patrol")]    
    [SerializeField] private float patrolPointReachDistance = 1f;

    private Transform[] patrolPoints;
    private Transform player;


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
        soundTimer += Time.deltaTime;
        if(soundTimer > soundDelay)
        {
            int rand =Random.Range(0,versiMumble.Length);
            audioSource.PlayOneShot(versiMumble[rand]);
            soundTimer = 0f;
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


    // Istanzia un proiettile enemy e lo spara verso il centro del player.
    private void Shoot()
    {
        if (bulletPrefab == null || firePoint == null || player == null)
            return;

        Vector3 targetPosition = player.position + Vector3.up * 0.5f;

        Vector3 shootDirection =
            (targetPosition - firePoint.position).normalized;

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

    // Riceve i patrol points dal manager di spawn.
    public void SetPatrolPoints(Transform[] newPatrolPoints)
    {
        patrolPoints = newPatrolPoints;

        GoToNextPatrolPoint();
    }

    // Riceve il player dal manager.
    public void SetPlayer(Transform newPlayer)
    {
        player = newPlayer;
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
