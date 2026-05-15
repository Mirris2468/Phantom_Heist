using UnityEngine;

public class SecurityCamera : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        EnemyMovement closestEnemy = FindClosestEnemy(other.transform.position);

        if (closestEnemy != null)
        {
            closestEnemy.InvestigatePosition(other.transform.position);

            Debug.Log("Enemigo enviado a investigar.");
        }
    }

    EnemyMovement FindClosestEnemy(Vector2 targetPosition)
    {
        EnemyMovement[] enemies = FindObjectsByType<EnemyMovement>(
            FindObjectsSortMode.None
        );

        EnemyMovement closest = null;
        float closestDistance = Mathf.Infinity;

        foreach (EnemyMovement enemy in enemies)
        {
            float distance = Vector2.Distance(
                enemy.transform.position,
                targetPosition
            );

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = enemy;
            }
        }

        return closest;
    }
}