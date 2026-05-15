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
    private Vector2 currentVelocity;

    private bool isRunning;
    private bool isSneaking;

    // Referencia al objeto interactuable actual
    private IInteractable currentInteractable;

    public bool IsMoving => moveInput != Vector2.zero;
    public bool IsRunning => isRunning;
    public bool IsSneaking => isSneaking;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // =========================
    // INPUT SYSTEM CALLBACKS
    // =========================

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
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
        else
        {
            Debug.Log("Nada para interactuar.");
        }
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
        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        // Si está en sigilo reducimos velocidad
        if (isSneaking)
        {
            currentSpeed *= 0.5f;
        }

        Vector2 targetVelocity = moveInput * currentSpeed;

        float smoothness = IsMoving
            ? acceleration
            : deceleration;

        currentVelocity = Vector2.Lerp(
            currentVelocity,
            targetVelocity,
            smoothness * Time.fixedDeltaTime
        );

        rb.linearVelocity = currentVelocity;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Entró en zona de sigilo
        if (other.CompareTag("SneakZone"))
        {
            isSneaking = true;
            Debug.Log("Entraste en sigilo.");
        }
        if (other.TryGetComponent(out IInteractable interactable))
        {
            currentInteractable = interactable;
            Debug.Log("Objeto interactuable cerca.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("SneakZone"))
        {
            isSneaking = false;
            Debug.Log("Saliste del sigilo.");
        }

        if (other.TryGetComponent(out IInteractable interactable))
        {
            if (currentInteractable == interactable)
            {
                currentInteractable = null;
                Debug.Log("Objeto interactuable fuera de alcance.");
            }
        }
    }
}