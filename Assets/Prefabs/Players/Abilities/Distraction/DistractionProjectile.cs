using UnityEngine;

public class DistractionProjectile : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 12f;

    [Header("Effect")]
    [SerializeField] private GameObject distractionBlastPrefab;

    private Vector2 direction;
    private GameObject owner;

    // =========================
    // INIT
    // =========================
    public void Initialize(Vector2 aimDirection, GameObject shooter)
    {
        direction = aimDirection.normalized;
        owner = shooter;

        float angle =
            Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    // =========================
    // MOVE
    // =========================
    private void Update()
    {
        transform.position +=
            (Vector3)(direction * speed * Time.deltaTime);
    }

    // =========================
    // COLLISION
    // =========================
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == owner)
            return;

        if (other.CompareTag("Player"))
            return;

        // Ignorar triggers (visión, áreas de detección, etc.)
        if (other.isTrigger)
            return;

        EnemyMovement enemy = other.GetComponent<EnemyMovement>();

        if (enemy != null)
        {
            enemy.InvestigatePosition(owner.transform.position);
        }
        else
        {
            Instantiate(
                distractionBlastPrefab,
                transform.position,
                Quaternion.identity
            );
        }

        Destroy(gameObject);
    }
}