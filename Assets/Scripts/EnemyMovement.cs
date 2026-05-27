using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [Header("Patrulla Base")]
    public Transform[] basePatrolPoints;

    [Header("Patrulla Alertada")]
    public Transform[] alertedPatrolPoints;

    [Header("Espera")]
    public float minWaitTime = 1f;
    public float maxWaitTime = 3f;

    [Header("Velocidades")]
    [SerializeField] private float patrolSpeed = 2f;
    [SerializeField] private float chaseSpeed = 4f;

    [Header("Investigacion")]
    [SerializeField] private float investigateWaitTime = 2f;

    [Header("Vision")]
    [SerializeField] private float viewDistance = 6f;
    [SerializeField] private float viewAngle = 90f;
    [SerializeField] private LayerMask obstacleMask;

    [Header("Knockout")]
    [SerializeField] private float knockoutTime = 5f;

    [Header("Rotacion")]
    [SerializeField] private float rotationSpeed = 10f;

    private NavMeshAgent agent;

    private int currentPointIndex = 0;

    private bool waiting;
    private float waitCounter;

    private Transform player;
    private bool isChasing;
    private bool isInvestigating;

    private Vector2 lastSeenPosition;

    private bool hasSeenPlayerBefore = false;

    // KO
    private bool isKnockedOut = false;
    private float knockoutTimer = 0f;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    private void Start()
    {
        GoToNextPatrolPoint();
    }

    private void Update()
    {
        HandleRotation();

        // =========================
        // KO (PRIORIDAD ABSOLUTA)
        // =========================
        if (isKnockedOut)
        {
            agent.ResetPath();

            knockoutTimer -= Time.deltaTime;

            if (knockoutTimer <= 0f)
            {
                RecoverFromKnockout();
            }

            return;
        }

        // =========================
        // CHASE
        // =========================
        if (player != null && isChasing)
        {
            if (!CanSeePlayer(player))
            {
                StartInvestigation(lastSeenPosition);
                return;
            }

            agent.speed = chaseSpeed;
            agent.SetDestination(player.position);

            lastSeenPosition = player.position;

            return;
        }

        // =========================
        // INVESTIGACION
        // =========================
        if (isInvestigating)
        {
            if (!agent.pathPending &&
                agent.remainingDistance < 0.2f)
            {
                isInvestigating = false;

                waiting = true;
                waitCounter = investigateWaitTime;

                agent.ResetPath();

                if (hasSeenPlayerBefore)
                    SwitchToAlertRoute();
            }

            return;
        }

        // =========================
        // ESPERA
        // =========================
        if (waiting)
        {
            waitCounter -= Time.deltaTime;

            if (waitCounter <= 0)
            {
                waiting = false;
                GoToNextPatrolPoint();
            }

            return;
        }

        // =========================
        // PATRULLA
        // =========================
        if (!agent.pathPending &&
            agent.remainingDistance < 0.2f)
        {
            waiting = true;

            waitCounter = Random.Range(minWaitTime, maxWaitTime);

            agent.ResetPath();
        }
    }

    // =========================
    // KO SYSTEM
    // =========================
    public void KnockOut(float duration)
    {
        isKnockedOut = true;
        knockoutTimer = duration;

        isChasing = false;
        isInvestigating = false;
        player = null;

        waiting = false;

        agent.ResetPath();
    }

    void RecoverFromKnockout()
    {
        isKnockedOut = false;

        // al despertar entra en alerta automáticamente
        if (hasSeenPlayerBefore)
        {
            currentPointIndex = 0;
        }

        GoToNextPatrolPoint();
    }

    // =========================
    // PATRULLA
    // =========================
    void GoToNextPatrolPoint()
    {
        Transform[] route = hasSeenPlayerBefore
            ? alertedPatrolPoints
            : basePatrolPoints;

        if (route.Length == 0)
            return;

        agent.speed = patrolSpeed;

        agent.SetDestination(route[currentPointIndex].position);

        currentPointIndex++;

        if (currentPointIndex >= route.Length)
            currentPointIndex = 0;
    }

    void SwitchToAlertRoute()
    {
        hasSeenPlayerBefore = true;
        currentPointIndex = 0;
    }

    // =========================
    // INVESTIGACION
    // =========================
    void StartInvestigation(Vector2 position)
    {
        isChasing = false;
        player = null;

        isInvestigating = true;
        waiting = false;

        agent.speed = patrolSpeed;
        agent.SetDestination(position);
    }

    public void InvestigatePosition(Vector2 position)
    {
        StartInvestigation(position);
    }

    // =========================
    // DETECCION
    // =========================
    private void OnTriggerStay2D(Collider2D other)
    {
        if (isKnockedOut)
            return;

        if (!other.CompareTag("Player"))
            return;

        PlayerMovement movement =
            other.GetComponent<PlayerMovement>();

        if (movement == null || movement.IsSneaking)
            return;

        if (CanSeePlayer(other.transform))
        {
            player = other.transform;
            isChasing = true;

            lastSeenPosition = player.position;
            hasSeenPlayerBefore = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        isChasing = false;
        player = null;

        GoToNextPatrolPoint();
    }

    // =========================
    // VISION
    // =========================
    bool CanSeePlayer(Transform target)
    {
        Vector2 origin = transform.position;

        Vector2 dir = target.position - transform.position;
        float distance = dir.magnitude;

        if (distance > viewDistance)
            return false;

        Vector2 dirNorm = dir.normalized;

        float angle = Vector2.Angle(transform.right, dirNorm);

        if (angle > viewAngle * 0.5f)
            return false;

        RaycastHit2D hit = Physics2D.Raycast(
            origin,
            dirNorm,
            distance,
            obstacleMask
        );

        if (hit.collider != null)
            return false;

        return true;
    }

    // =========================
    // ROTACION
    // =========================
    void HandleRotation()
    {
        Vector3 velocity = agent.velocity;

        if (velocity.sqrMagnitude > 0.01f)
        {
            float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;

            Quaternion targetRot = Quaternion.Euler(0, 0, angle);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRot,
                rotationSpeed * Time.deltaTime
            );
        }
    }
}