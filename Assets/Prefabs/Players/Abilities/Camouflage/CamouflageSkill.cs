using System.Collections;
using UnityEngine;

public class CamouflageSkill : MonoBehaviour, IAbility, IResettableAbility
{
    [Header("References")]
    [SerializeField] private GameObject sneakZonePrefab;

    [Header("Usage")]
    [SerializeField] private int maxUses = 3;
    [SerializeField] private float duration = 5f;
    [SerializeField] private float cooldown = 10f;

    private int currentUses;

    private bool isActive;
    private bool isOnCooldown;

    private GameObject activeSneakZone;

    private Coroutine activeRoutine;
    private Coroutine cooldownRoutine;

    public bool IsAiming => false;

    private void Awake()
    {
        currentUses = maxUses;
    }

    private void Update()
    {
        if (activeSneakZone != null)
        {
            activeSneakZone.transform.position = transform.position;
        }
    }

    // =========================
    // PRESS
    // =========================
    public void Press()
    {
        // Si está activa, se cancela inmediatamente
        if (isActive)
        {
            EndCamouflage();
            return;
        }

        if (currentUses <= 0)
        {
            Debug.Log("No Camouflage uses left!");
            return;
        }

        if (isOnCooldown)
        {
            Debug.Log("Camouflage on cooldown!");
            return;
        }

        ActivateCamouflage();
    }

    // =========================
    // CANCEL
    // =========================
    public void Cancel()
    {
        // Esta habilidad no utiliza apuntado.
    }

    // =========================
    // ACTIVATE
    // =========================
    private void ActivateCamouflage()
    {
        isActive = true;

        currentUses--;

        activeSneakZone = Instantiate(
            sneakZonePrefab,
            transform.position,
            Quaternion.identity
        );

        activeRoutine = StartCoroutine(ActiveRoutine());

        Debug.Log($"Camouflage Activated. Remaining Uses: {currentUses}");
    }

    // =========================
    // ACTIVE TIMER
    // =========================
    private IEnumerator ActiveRoutine()
    {
        yield return new WaitForSeconds(duration);

        EndCamouflage();
    }

    // =========================
    // END
    // =========================
    private void EndCamouflage()
    {
        if (!isActive)
            return;

        isActive = false;

        if (activeRoutine != null)
        {
            StopCoroutine(activeRoutine);
            activeRoutine = null;
        }

        if (activeSneakZone != null)
        {
            Destroy(activeSneakZone);
            activeSneakZone = null;
        }

        if (cooldownRoutine != null)
        {
            StopCoroutine(cooldownRoutine);
        }

        cooldownRoutine = StartCoroutine(CooldownRoutine());

        Debug.Log("Camouflage Ended");
    }

    // =========================
    // COOLDOWN
    // =========================
    private IEnumerator CooldownRoutine()
    {
        isOnCooldown = true;

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

        isActive = false;
        isOnCooldown = false;

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

        if (activeSneakZone != null)
        {
            Destroy(activeSneakZone);
            activeSneakZone = null;
        }

        Debug.Log("Camouflage Uses Reset");
    }
}