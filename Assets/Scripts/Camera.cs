using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Follow")]
    [SerializeField] private float followSmoothness = 8f;

    [Header("Look Ahead")]
    [SerializeField] private float lookAheadResponsiveness = 10f;

    private Rigidbody2D targetRb;
    private PlayerMovement playerMovement;

    private Vector3 currentLookAhead;

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;

        targetRb = target.GetComponent<Rigidbody2D>();
        playerMovement = target.GetComponent<PlayerMovement>();
    }

    private void LateUpdate()
    {
        if (target == null || targetRb == null)
            return;

        UpdateLookAhead();

        Vector3 desiredPosition =
            target.position +
            currentLookAhead;

        desiredPosition.z = transform.position.z;

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            followSmoothness * Time.deltaTime
        );
    }

    private void UpdateLookAhead()
    {
        Vector2 velocity = targetRb.linearVelocity;

        Vector3 desiredLookAhead = Vector3.zero;

        if (velocity.magnitude > 0.05f)
        {
            float currentLookAheadDistance;

            if (playerMovement.IsSneaking)
            {
                currentLookAheadDistance = 0.5f;
            }
            else if (playerMovement.IsRunning)
            {
                currentLookAheadDistance = 4f;
            }
            else
            {
                currentLookAheadDistance = 2f;
            }

            desiredLookAhead =
                (Vector3)velocity.normalized *
                currentLookAheadDistance;
        }

        currentLookAhead = Vector3.Lerp(
            currentLookAhead,
            desiredLookAhead,
            lookAheadResponsiveness * Time.deltaTime
        );
    }
}