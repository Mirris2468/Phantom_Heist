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

    private Rigidbody2D rb;

    private Vector2 moveInput;
    private Vector2 finalInput;
    private Vector2 currentVelocity;

    private bool isRunning;
    private bool isSneaking;

    private IInteractable currentInteractable;

    public PlayerSkillManager skillManager;
    public EMPGunSkill empGun;

    public bool IsAiming { get; set; }
    public Vector2 AimDirection { get; private set; } = Vector2.right;

    public bool IsRunning => isRunning;
    public bool IsSneaking => isSneaking;
    public bool IsMoving => moveInput != Vector2.zero;

    public static PlayerMovement LocalPlayer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        LocalPlayer = this;
    }

    // =========================
    // ACTIONS (UI + INPUT COMMON LOGIC)
    // =========================

    public void UseAbility1()
    {
        skillManager?.UseAbility1();
    }

    public void UseAbility2()
    {
        skillManager?.UseAbility2();
    }

    public void UseInteract()
    {
        currentInteractable?.Interact();
    }

    public void ToggleRun()
    {
        isRunning = !isRunning;
    }

    // =========================
    // INPUT SYSTEM (PC)
    // =========================

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        if (moveInput != Vector2.zero)
            AimDirection = moveInput.normalized;
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        isRunning = context.ReadValueAsButton();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        UseInteract();
    }

    public void OnAbility1(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        UseAbility1();
    }

    public void OnAbility2(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        UseAbility2();
    }

    // =========================
    // MOBILE + PC MERGE
    // =========================

    private void Update()
    {
        Vector2 inputMobile = MobileInput.Instance != null
            ? MobileInput.Instance.Move
            : Vector2.zero;

        Vector2 inputPC = moveInput;

        finalInput = inputMobile.sqrMagnitude > 0.01f
            ? inputMobile
            : inputPC;

        if (finalInput != Vector2.zero)
            AimDirection = finalInput.normalized;
    }

    // =========================
    // MOVEMENT PHYSICS
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

        float speed = isRunning ? runSpeed : walkSpeed;

        if (isSneaking)
            speed *= 0.5f;

        Vector2 targetVelocity = finalInput * speed;

        float smoothness = finalInput != Vector2.zero
            ? acceleration
            : deceleration;

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
            currentInteractable = interactable;

        if (other.CompareTag("SneakZone"))
            isSneaking = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out IInteractable interactable))
        {
            if (currentInteractable == interactable)
                currentInteractable = null;
        }

        if (other.CompareTag("SneakZone"))
            isSneaking = false;
    }
}