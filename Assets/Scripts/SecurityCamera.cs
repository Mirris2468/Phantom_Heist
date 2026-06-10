using UnityEngine;
using System.Collections;

public class SecurityCamera : MonoBehaviour
{
    [Header("Rotación")]
    [SerializeField] private float rotationAngle = 30f;
    [SerializeField] private float rotationSpeed = 2f;
    [SerializeField] private float waitTime = 1f;

    private Quaternion baseRotation;
    private Quaternion leftRotation;
    private Quaternion rightRotation;

    private void Start()
    {
        // 🔥 ESTA es la clave: tomamos la rotación real del editor
        baseRotation = transform.rotation;

        leftRotation = baseRotation * Quaternion.Euler(0f, 0f, rotationAngle);
        rightRotation = baseRotation * Quaternion.Euler(0f, 0f, -rotationAngle);

        transform.rotation = rightRotation;

        StartCoroutine(RotateLoop());
    }

    private IEnumerator RotateLoop()
    {
        while (true)
        {
            yield return RotateTo(leftRotation);
            yield return new WaitForSeconds(waitTime);

            yield return RotateTo(rightRotation);
            yield return new WaitForSeconds(waitTime);
        }
    }

    private IEnumerator RotateTo(Quaternion target)
    {
        while (Quaternion.Angle(transform.rotation, target) > 0.1f)
        {
            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                target,
                Time.deltaTime * rotationSpeed
            );

            yield return null;
        }

        transform.rotation = target;
    }

// =========================
// TU LÓGICA ORIGINAL
// =========================

private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        EnemyMovement closestEnemy = FindClosestEnemy(other.transform.position);

        if (closestEnemy != null)
        {
            closestEnemy.InvestigatePosition(other.transform.position);
            Debug.Log("Cámara: enemigo enviado a investigar.");
        }
    }

    EnemyMovement FindClosestEnemy(Vector2 targetPosition)
    {
        EnemyMovement[] enemies =
            FindObjectsByType<EnemyMovement>(FindObjectsSortMode.None);

        EnemyMovement closest = null;
        float closestDistance = Mathf.Infinity;

        foreach (EnemyMovement enemy in enemies)
        {
            float distance = Vector2.Distance(enemy.transform.position, targetPosition);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = enemy;
            }
        }

        return closest;
    }
}