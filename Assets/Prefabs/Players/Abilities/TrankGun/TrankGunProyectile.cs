using UnityEngine;

public class TrankGunProyectile : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 12f;

    [Header("Effect")]
    [SerializeField] private float knockoutDuration = 5f;

    private Vector2 direction;
    private GameObject owner;

    // =========================
    // INIT (called by weapon)
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

        EnemyMovement enemy = other.GetComponent<EnemyMovement>();

        if (enemy != null)
        {
            enemy.Knockout(knockoutDuration);
        }

        Destroy(gameObject);
    }
}