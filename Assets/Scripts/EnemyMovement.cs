using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

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

    [Header("Investigación")]
    [SerializeField] private float investigateWaitTime = 2f;

    [Header("Vision")]
    [SerializeField] private float viewDistance = 6f;
    [SerializeField] private float viewAngle = 90f;
    [SerializeField] private LayerMask obstacleMask;

    [Header("Rotación")]
    [SerializeField] private float rotationSpeed = 10f;

    [Header("Detección")]
    [SerializeField] private float detectionTime = 2f;

    [Header("Game Over")]
    [SerializeField] private Animator animator;
    [SerializeField] private string sceneToLoad;
    [SerializeField] private float delayBeforeLoad = 1f;

    private NavMeshAgent agent;

    private int currentPointIndex = 0;

    private bool waiting;
    private float waitCounter;

    private bool isInvestigating;
    private bool isDetectingPlayer;

    private bool hasSeenPlayerBefore;

    private float detectionTimer;

    private bool suspicionTriggered;
    private bool gameOverTriggered;

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

        if (gameOverTriggered)
            return;

        if (isDetectingPlayer)
            return;

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

        if (!agent.pathPending &&
            agent.remainingDistance < 0.2f)
        {
            waiting = true;

            waitCounter = Random.Range(
                minWaitTime,
                maxWaitTime
            );

            agent.ResetPath();
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (gameOverTriggered)
            return;

        if (!other.CompareTag("Player"))
            return;

        PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
        if (playerMovement != null && playerMovement.IsSneaking)
            return;

        if (!CanSeePlayer(other.transform))
        {
            if (detectionTimer > 0f || isDetectingPlayer)
            {
                ResetDetection();
            }

            return;
        }

        detectionTimer += Time.deltaTime;

        HUDManager.Instance?.ShowDetection(
            detectionTimer,
            detectionTime
        );

        if (!suspicionTriggered)
        {
            isDetectingPlayer = true;

            agent.ResetPath();
        }

        // Sospecha
        if (!suspicionTriggered &&
            detectionTimer >= detectionTime * 0.5f)
        {
            suspicionTriggered = true;

            hasSeenPlayerBefore = true;

            GameManager.Instance?.AddSuspicion(1);

            SwitchToAlertRoute();
        }

        // Game Over
        if (detectionTimer >= detectionTime)
        {
            TriggerGameOver();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        // Solo resetear si realmente detectaba
        if (detectionTimer > 0f || isDetectingPlayer)
        {
            ResetDetection();
        }
    }

    void ResetDetection()
    {
        bool wasDetecting = isDetectingPlayer;

        detectionTimer = 0f;

        isDetectingPlayer = false;

        HUDManager.Instance?.HideDetection();

        // Solo retomar patrulla si realmente estaba detenido
        if (wasDetecting &&
            !waiting &&
            !isInvestigating &&
            !gameOverTriggered)
        {
            GoToNextPatrolPoint();
        }
    }

    public void InvestigatePosition(Vector2 position)
    {
        if (gameOverTriggered)
            return;

        isInvestigating = true;

        isDetectingPlayer = false;

        waiting = false;

        agent.ResetPath();

        agent.speed = patrolSpeed;

        agent.SetDestination(position);
    }

    void GoToNextPatrolPoint()
    {
        Transform[] route = hasSeenPlayerBefore
            ? alertedPatrolPoints
            : basePatrolPoints;

        if (route == null || route.Length == 0)
            return;

        agent.speed = patrolSpeed;

        agent.SetDestination(
            route[currentPointIndex].position
        );

        currentPointIndex++;

        if (currentPointIndex >= route.Length)
            currentPointIndex = 0;
    }

    void SwitchToAlertRoute()
    {
        hasSeenPlayerBefore = true;

        currentPointIndex = 0;

        waiting = false;

        isInvestigating = false;

        isDetectingPlayer = false;

        GoToNextPatrolPoint();
    }

    void TriggerGameOver()
    {
        if (gameOverTriggered)
            return;

        gameOverTriggered = true;

        agent.ResetPath();

        HUDManager.Instance?.HideDetection();

        if (animator != null)
            animator.SetTrigger("Activate");

        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(delayBeforeLoad);

        GameManager.Instance?.SaveResults(false);

        SceneManager.LoadScene(sceneToLoad);
    }

    bool CanSeePlayer(Transform target)
    {
        Vector2 origin = transform.position;

        Vector2 dir =
            target.position - transform.position;

        float dist = dir.magnitude;

        if (dist > viewDistance)
            return false;

        float angle = Vector2.Angle(
            transform.right,
            dir.normalized
        );

        if (angle > viewAngle * 0.5f)
            return false;

        RaycastHit2D hit = Physics2D.Raycast(
            origin,
            dir.normalized,
            dist,
            obstacleMask
        );

        return hit.collider == null;
    }

    void HandleRotation()
    {
        Vector3 vel = agent.velocity;

        if (vel.sqrMagnitude > 0.01f)
        {
            float ang =
                Mathf.Atan2(vel.y, vel.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.Euler(0, 0, ang),
                rotationSpeed * Time.deltaTime
            );
        }
    }
}