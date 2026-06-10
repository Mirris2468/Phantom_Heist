using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 8f;

    [Header("Smoothing")]
    [SerializeField] private float acceleration = 20f;
    [SerializeField] private float deceleration = 25f;
    public bool IsMoving => moveInput != Vector2.zero;

    private Rigidbody2D rb;

    private Vector2 moveInput;
    private Vector2 currentVelocity;

    private bool isRunning;
    private bool isSneaking;

    // Interacción
    private IInteractable currentInteractable;

    // Skills
    public PlayerSkillManager skillManager;
    public EMPGunSkill empGun;

    // Estado de aiming global (bloquea movimiento)
    public bool IsAiming { get; set; }

    public Vector2 AimDirection { get; private set; } = Vector2.right;

    public bool IsRunning => isRunning;
    public bool IsSneaking => isSneaking;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // =========================
    // INPUT SYSTEM
    // =========================

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        if (moveInput != Vector2.zero)
        {
            AimDirection = moveInput.normalized;
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        isRunning = context.ReadValueAsButton();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        if (currentInteractable != null)
        {
            currentInteractable.Interact();
        }
    }

    public void OnAbility1(InputAction.CallbackContext context)
    {
        Debug.Log($"ABILITY1 PHASE: {context.phase} / frame {Time.frameCount}");

        if (!context.performed)
            return;

        skillManager.UseAbility1();
    }

    public void OnAbility2(InputAction.CallbackContext context)
    {
        Debug.Log($"ABILITY2 PHASE: {context.phase} / frame {Time.frameCount}");

        if (!context.performed)
            return;

        skillManager.UseAbility2();
    }

    // helper seguro
    private bool skillManagerHasAiming()
    {
        return empGun != null && empGun.IsAiming;
    }

    // =========================
    // MOVEMENT
    // =========================

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if (IsAiming)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        if (isSneaking)
            currentSpeed *= 0.5f;

        Vector2 targetVelocity = moveInput * currentSpeed;

        float smoothness =
            IsMoving ? acceleration : deceleration;

        currentVelocity = Vector2.Lerp(
            currentVelocity,
            targetVelocity,
            smoothness * Time.fixedDeltaTime
        );

        rb.linearVelocity = currentVelocity;
    }

    // =========================
    // TRIGGERS
    // =========================

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out IInteractable interactable))
        {
            currentInteractable = interactable;
        }
        if (other.CompareTag("SneakZone"))
        {
            isSneaking = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out IInteractable interactable))
        {
            if (currentInteractable == interactable)
            {
                currentInteractable = null;
            }
        }
        if (other.CompareTag("SneakZone"))
        {
            isSneaking = false;
        }
    }
}