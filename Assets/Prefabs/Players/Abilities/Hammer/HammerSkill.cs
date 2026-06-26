using System.Collections;
using UnityEngine;

public class HammerSkill : MonoBehaviour, IAbility, IResettableAbility
{
    [Header("Settings")]
    [SerializeField] private int maxUses = 3;
    [SerializeField] private float cooldown = 2f;
    [SerializeField] private float breakDistance = 1.5f;
    [SerializeField] private LayerMask wallLayer; // SOLO BreakableWalls

    private int currentUses;
    private bool isOnCooldown;

    private PlayerMovement movement;

    public bool IsAiming => false;

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        currentUses = maxUses;
    }

    public void Press()
    {
        if (currentUses <= 0)
        {
            Debug.Log("No hammer uses left");
            return;
        }

        if (isOnCooldown)
        {
            Debug.Log("Hammer is on cooldown");
            return;
        }

        TryBreakWall();
    }

    public void Cancel()
    {
        // no aplica
    }

    private void TryBreakWall()
    {
        Vector2 dir = movement.AimDirection;

        if (dir == Vector2.zero)
            return;

        dir.Normalize();

        Vector2 origin = (Vector2)transform.position + dir * 0.25f;
        Debug.Log("LayerMask value: " + wallLayer.value);

        RaycastHit2D hit = Physics2D.Raycast(
            origin,
            dir,
            breakDistance,
            wallLayer
        );

        if (!hit)
        {
            Debug.Log("No wall hit");
            return;
        }

        IBreakable breakable = hit.collider.GetComponentInParent<IBreakable>();

        if (breakable == null)
        {
            Debug.Log("Hit object is not breakable: " + hit.collider.name);
            return;
        }

        breakable.Break();

        currentUses--;

        StartCoroutine(CooldownRoutine());
    }

    private IEnumerator CooldownRoutine()
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(cooldown);
        isOnCooldown = false;
    }

    public void ResetUses()
    {
        currentUses = maxUses;
        isOnCooldown = false;
    }
}