using System.Collections;
using UnityEngine;

public class AdrenalineSkill : MonoBehaviour, IAbility, IResettableAbility
{
    [Header("Usage")]
    [SerializeField] private int maxUses = 3;

    [SerializeField] private float duration = 5f;
    [SerializeField] private float cooldown = 10f;

    [Header("Effect")]
    [SerializeField] private float speedMultiplier = 2f;

    private int currentUses;

    private bool isActive;
    private bool isOnCooldown;

    private PlayerMovement movement;

    private Coroutine activeRoutine;
    private Coroutine cooldownRoutine;

    public bool IsAiming => false;

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();

        currentUses = maxUses;
    }

    // =========================
    // PRESS
    // =========================
    public void Press()
    {
        if (isActive)
            return;

        if (currentUses <= 0)
        {
            Debug.Log("No Adrenaline uses left!");
            return;
        }

        if (isOnCooldown)
        {
            Debug.Log("Adrenaline on cooldown!");
            return;
        }

        Activate();
    }

    // =========================
    // CANCEL
    // =========================
    public void Cancel()
    {
        // Esta habilidad no utiliza apuntado
        // y no puede cancelarse.
    }

    // =========================
    // ACTIVATE
    // =========================
    private void Activate()
    {
        isActive = true;
        isOnCooldown = true;

        currentUses--;

        movement.SetSpeedMultiplier(speedMultiplier);

        activeRoutine = StartCoroutine(ActiveRoutine());
        cooldownRoutine = StartCoroutine(CooldownRoutine());

        Debug.Log($"Adrenaline Activated. Remaining Uses: {currentUses}");
    }

    // =========================
    // EFFECT TIMER
    // =========================
    private IEnumerator ActiveRoutine()
    {
        yield return new WaitForSeconds(duration);

        movement.SetSpeedMultiplier(1f);

        isActive = false;

        activeRoutine = null;
    }

    // =========================
    // COOLDOWN
    // =========================
    private IEnumerator CooldownRoutine()
    {
        yield return new WaitForSeconds(cooldown);

        isOnCooldown = false;

        cooldownRoutine = null;
    }

    // =========================
    // RESET
    // =========================
    public void ResetUses()
    {
        currentUses = maxUses;

        if (activeRoutine != null)
        {
            StopCoroutine(activeRoutine);
            activeRoutine = null;
        }

        if (cooldownRoutine != null)
        {
            StopCoroutine(cooldownRoutine);
            cooldownRoutine = null;
        }

        movement.SetSpeedMultiplier(1f);

        isActive = false;
        isOnCooldown = false;

        Debug.Log("Adrenaline Uses Reset");
    }
}