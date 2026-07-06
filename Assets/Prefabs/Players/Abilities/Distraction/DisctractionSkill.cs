using System.Collections;
using UnityEngine;

public class DistractionSkill : MonoBehaviour, IAbility, IResettableAbility
{
    [Header("References")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;

    [Header("Usage")]
    [SerializeField] private int maxUses = 5;
    [SerializeField] private float cooldown = 15f;

    private int currentUses;
    private bool aiming;
    private bool isOnCooldown;

    private PlayerMovement movement;

    public bool IsAiming => aiming;

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();

        currentUses = maxUses;

        Debug.Log($"Distraction Awake - Uses: {currentUses}");
    }

    // =========================
    // CLICK IZQUIERDO
    // =========================
    public void Press()
    {
        if (currentUses <= 0)
        {
            Debug.Log("No Distraction uses left!");
            return;
        }

        if (isOnCooldown)
        {
            Debug.Log("Distraction on cooldown!");
            return;
        }

        // Primer click -> apuntar
        if (!aiming)
        {
            StartAim();
            return;
        }

        // Segundo click -> disparar
        Fire();
    }

    // =========================
    // CLICK DERECHO
    // =========================
    public void Cancel()
    {
        if (!aiming)
            return;

        aiming = false;

        if (movement != null)
            movement.IsAiming = false;

        Debug.Log("Distractiom aim cancelled");
    }

    // =========================
    // APUNTAR
    // =========================
    private void StartAim()
    {
        aiming = true;

        if (movement != null)
            movement.IsAiming = true;

        Debug.Log("Distraction aiming started");
    }

    // =========================
    // DISPARAR
    // =========================
    private void Fire()
    {
        aiming = false;

        if (movement != null)
            movement.IsAiming = false;

        GameObject projectile = Instantiate(
            projectilePrefab,
            firePoint.position,
            Quaternion.identity
        );

        DistractionProjectile projectileScript =
    projectile.GetComponent<DistractionProjectile>();

        if (projectileScript != null)
        {
            projectileScript.Initialize(
                movement.AimDirection,
                gameObject
            );
        }

        currentUses--;

        Debug.Log($"Distraction Fired. Remaining Uses: {currentUses}");

        StartCoroutine(CooldownRoutine());
    }

    // =========================
    // COOLDOWN
    // =========================
    private IEnumerator CooldownRoutine()
    {
        isOnCooldown = true;

        yield return new WaitForSeconds(cooldown);

        isOnCooldown = false;
    }

    // =========================
    // REINICIO DE NIVEL
    // =========================
    public void ResetUses()
    {
        currentUses = maxUses;

        aiming = false;
        isOnCooldown = false;

        if (movement != null)
            movement.IsAiming = false;

        Debug.Log("Distraction Uses Reset");
    }
}