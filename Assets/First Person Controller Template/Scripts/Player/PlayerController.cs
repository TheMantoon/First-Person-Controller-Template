using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    public float CurrentSpeed01 { get; private set; }
    public Vector3 Velocity => controller.velocity;
    public bool IsCrouching => isCrouching;
    public bool OnGround => controller.isGrounded;
    [SerializeField] private Slider staminaSlider = null;
    [SerializeField] private CanvasGroup staminaCanvasGroup = null;
    public Transform cameraPivot = null;
    [SerializeField] private float standingEyeHeight = 1.75f, crouchEyeHeight = 0.75f;
    public float walkSpeed = 4, sprintSpeed = 6.8f, crouchSpeed = 2.8f;
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float staminaDrainSpeed = 20f;
    [SerializeField] private float staminaRegenSpeed = 15f;
    [SerializeField] private float staminaCooldown = 1f;
    [SerializeField] private float standingHeight = 1.75f, crouchHeight = 1f;
    [SerializeField] private float crouchTransitionSpeed = 10f;
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float gravity = -20;
    [SerializeField] private bool useSprint = true;
    [SerializeField] private bool useStamina = true;
    [SerializeField] private bool useJump = true;
    [SerializeField] private bool useCrouch = true;
    [SerializeField] private LayerMask obstacleMask = ~0;
    private CharacterController controller = null;
    private Vector3 velocity = new Vector3();
    private bool isCrouching = false;
    private float stamina;
    private float regenBlockedUntil;
    private bool crouchToggled;

    private void Awake()
    {
        Instance = this;
        controller = GetComponent<CharacterController>();
        controller.center = new Vector3(0, controller.height / 2f, 0);
        Vector3 camPos = cameraPivot.localPosition;
        camPos.y = standingEyeHeight;
        cameraPivot.localPosition = camPos;
        stamina = maxStamina;
        staminaSlider.maxValue = maxStamina;
        staminaSlider.value = stamina;
    }

    private void Update()
    {
        UpdateCrouch();
        UpdateStamina();
        Move();
        HandleCrouch();
    }

    private void Move()
    {
        Vector2 input = InputManager.GetVector2("Move");
        if (InputManager.GetButton("Sprint") && input == Vector2.zero) input = Vector2.up;
        bool canSprint = stamina > 0 && !isCrouching && useSprint && InputManager.GetButton("Sprint");
        float speed = isCrouching ? crouchSpeed : canSprint ? sprintSpeed : walkSpeed;
        if (controller.isGrounded && velocity.y < 0) velocity.y = -2f;
        if (useJump && InputManager.GetButton("Jump") && controller.isGrounded) velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        velocity.y += gravity * Time.deltaTime;
        Vector3 horizontal = (transform.right * input.x + transform.forward * input.y) * speed;
        CurrentSpeed01 = horizontal.magnitude / sprintSpeed;
        Vector3 vertical = Vector3.up * velocity.y;
        controller.Move((horizontal + vertical) * Time.deltaTime);
    }

    private void UpdateCrouch()
    {
        if (!useCrouch)
        {
            isCrouching = false;
            return;
        }
        if (ApplicationControl.Instance.GetCrouchMode() == CrouchMode.Hold) isCrouching = InputManager.GetButton("Crouch");
        else
        {
            if (InputManager.GetButtonDown("Crouch"))
            {
                if (!crouchToggled) crouchToggled = true;
                else if (CanStand()) crouchToggled = false;
            }
            isCrouching = crouchToggled;
        }
        if (!isCrouching && !CanStand()) isCrouching = true;
    }

    private void UpdateStamina()
    {
        if (useSprint && !useStamina)
        {
            stamina = maxStamina;
            staminaCanvasGroup.alpha = 0;
            return;
        }
        bool sprinting = InputManager.GetButton("Sprint") && !isCrouching && CurrentSpeed01 > 0.1f;
        if (sprinting && stamina > 0)
        {
            stamina -= staminaDrainSpeed * Time.deltaTime;
            if (stamina <= 0)
            {
                stamina = 0;
                regenBlockedUntil = Time.time + staminaCooldown;
            }
        }
        else if (Time.time >= regenBlockedUntil)
        {
            stamina += staminaRegenSpeed * Time.deltaTime;
            stamina = Mathf.Min(stamina, maxStamina);
        }
        staminaSlider.value = stamina;
        float targetAlpha = stamina < maxStamina - 0.01f ? 1f : 0f;
        staminaCanvasGroup.alpha = Mathf.Lerp(staminaCanvasGroup.alpha, targetAlpha, Time.deltaTime * 5f);
    }

    private void HandleCrouch()
    {
        if (!controller.isGrounded) return;
        float targetHeight = isCrouching ? crouchHeight : standingHeight;
        float targetEyeHeight = isCrouching ? crouchEyeHeight : standingEyeHeight;
        controller.height = Mathf.Lerp(controller.height, targetHeight, crouchTransitionSpeed * Time.deltaTime);
        controller.center = new Vector3(0, controller.height / 2f, 0);
        Vector3 camPos = cameraPivot.localPosition;
        camPos.y = Mathf.Lerp(camPos.y, targetEyeHeight, crouchTransitionSpeed * Time.deltaTime);
        cameraPivot.localPosition = camPos;
    }

    private bool CanStand()
    {
        float scaleY = transform.lossyScale.y;
        float scaleX = transform.lossyScale.x;
        float radius = controller.radius * scaleX;
        float height = standingHeight * scaleY;
        Vector3 center = transform.position + Vector3.up * (height / 2f);
        return !Physics.CheckCapsule(center + Vector3.up * (height / 2f - radius),
            center - Vector3.up * (height / 2f - radius), radius,
            obstacleMask, QueryTriggerInteraction.Ignore);
    }
}