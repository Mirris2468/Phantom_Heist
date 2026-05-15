using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Patrulla")]
    public Transform[] patrolPoints;
    public float moveSpeed = 2f;

    [Header("Espera")]
    public float minWaitTime = 1f;
    public float maxWaitTime = 3f;

    [Header("Vision")]
    [SerializeField] private float chaseSpeed = 4f;

    [Header("Investigacion")]
    [SerializeField] private float investigateWaitTime = 2f;

    private int currentPointIndex = 0;

    private bool waiting = false;
    private float waitCounter;

    private Transform player;
    private PlayerMovement playerMovement;

    private bool isChasing;

    private bool isInvestigating;
    private Vector2 investigateTarget;

    private void Update()
    {
        if (patrolPoints.Length == 0)
            return;

        if (isInvestigating)
        {
            Investigate();
            return;
        }

        if (isChasing && player != null)
        {
            ChasePlayer();
            return;
        }

        if (waiting)
        {
            waitCounter -= Time.deltaTime;

            if (waitCounter <= 0)
            {
                waiting = false;

                currentPointIndex++;

                if (currentPointIndex >= patrolPoints.Length)
                    currentPointIndex = 0;
            }

            return;
        }

        MoveToPoint();
    }


    void MoveToPoint()
    {
        Transform targetPoint = patrolPoints[currentPointIndex];

        transform.position = Vector2.MoveTowards(
            transform.position,
            targetPoint.position,
            moveSpeed * Time.deltaTime
        );

        float distance = Vector2.Distance(
            transform.position,
            targetPoint.position
        );

        if (distance < 0.1f)
        {
            waiting = true;

            waitCounter = Random.Range(
                minWaitTime,
                maxWaitTime
            );
        }
    }


    void ChasePlayer()
    {
        transform.position = Vector2.MoveTowards(
            transform.position,
            player.position,
            chaseSpeed * Time.deltaTime
        );
    }


    public void InvestigatePosition(Vector2 targetPosition)
    {
        investigateTarget = targetPosition;

        isInvestigating = true;

        isChasing = false;
        waiting = false;
    }

    void Investigate()
    {
        transform.position = Vector2.MoveTowards(
            transform.position,
            investigateTarget,
            moveSpeed * Time.deltaTime
        );

        float distance = Vector2.Distance(
            transform.position,
            investigateTarget
        );

        if (distance < 0.1f)
        {
            isInvestigating = false;

            waiting = true;
            waitCounter = investigateWaitTime;
        }
    }


    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        PlayerMovement movement =
            other.GetComponent<PlayerMovement>();

        if (movement != null && !movement.IsSneaking)
        {
            player = other.transform;
            playerMovement = movement;

            isChasing = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        isChasing = false;

        player = null;
        playerMovement = null;
    }
}