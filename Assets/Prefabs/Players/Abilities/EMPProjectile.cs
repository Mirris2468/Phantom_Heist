using UnityEngine;

public class EMPProjectile : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 12f;

    [Header("Effect")]
    [SerializeField] private GameObject empBlastPrefab;

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
        // Ignorar al que disparó el proyectil
        if (other.gameObject == owner)
            return;

        // Ignorar al jugador si el EMP no debe afectar contacto directo
        if (other.CompareTag("Player"))
            return;

        Explode();
    }

    // =========================
    // EXPLOSION
    // =========================
    private void Explode()
    {
        if (empBlastPrefab != null)
        {
            Instantiate(empBlastPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}