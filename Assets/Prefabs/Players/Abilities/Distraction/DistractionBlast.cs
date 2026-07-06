using UnityEngine;

public class DistractionBlast : MonoBehaviour
{
    private void Start()
    {
        float radius = transform.localScale.x * 0.5f;

        Collider2D[] hits =
            Physics2D.OverlapCircleAll(
                transform.position,
                radius
            );

        EnemyMovement closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider2D hit in hits)
        {
            // Ignorar triggers (visión, detección, etc.)
            if (hit.isTrigger)
                continue;

            EnemyMovement enemy =
                hit.GetComponent<EnemyMovement>();

            if (enemy == null)
                continue;

            float distance = Vector2.Distance(
                transform.position,
                enemy.transform.position
            );

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }

        if (closestEnemy != null)
        {
            closestEnemy.InvestigatePosition(
                transform.position
            );
        }

        Destroy(gameObject, 0.2f);
    }
}