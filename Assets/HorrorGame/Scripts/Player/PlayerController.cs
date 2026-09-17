using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 4.5f;
    public float jumpHeight = 1.1f;
    public float gravity = -22f;
    public float dashSpeed = 17f;
    public float dashDuration = 0.18f;
    public float dashCooldown = 2f;

    CharacterController controller;
    Vector3 dashDirection;
    float verticalSpeed;
    float dashTimeLeft;
    float cooldownTimeLeft;

    public bool IsDashing => dashTimeLeft > 0f;
    public bool IsMoving { get; private set; }
    public bool IsGrounded => controller.isGrounded;
    public float DashCharge => cooldownTimeLeft <= 0f ? 1f : 1f - cooldownTimeLeft / dashCooldown;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (cooldownTimeLeft > 0f) cooldownTimeLeft -= Time.deltaTime;

        Vector2 input = Vector2.zero;
        bool jumpPressed = false;
        bool dashPressed = false;

        if (GameManager.PlayerInputEnabled && Keyboard.current != null)
        {
            Keyboard keyboard = Keyboard.current;
            if (keyboard.wKey.isPressed) input.y += 1f;
            if (keyboard.sKey.isPressed) input.y -= 1f;
            if (keyboard.dKey.isPressed) input.x += 1f;
            if (keyboard.aKey.isPressed) input.x -= 1f;
            jumpPressed = keyboard.spaceKey.wasPressedThisFrame;
            dashPressed = keyboard.leftShiftKey.wasPressedThisFrame;
        }

        input = Vector2.ClampMagnitude(input, 1f);
        IsMoving = input.sqrMagnitude > 0.01f;

        Vector3 direction = transform.right * input.x + transform.forward * input.y;

        if (dashPressed && cooldownTimeLeft <= 0f)
        {
            dashDirection = direction.sqrMagnitude > 0.01f ? direction.normalized : transform.forward;
            dashTimeLeft = dashDuration;
            cooldownTimeLeft = dashCooldown;
        }

        if (controller.isGrounded && verticalSpeed < 0f) verticalSpeed = -2f;
        if (jumpPressed && controller.isGrounded) verticalSpeed = Mathf.Sqrt(jumpHeight * -2f * gravity);
        verticalSpeed += gravity * Time.deltaTime;

        Vector3 horizontalMove;
        if (dashTimeLeft > 0f)
        {
            dashTimeLeft -= Time.deltaTime;
            horizontalMove = dashDirection * dashSpeed;
        }
        else
        {
            horizontalMove = direction * walkSpeed;
        }

        controller.Move((horizontalMove + Vector3.up * verticalSpeed) * Time.deltaTime);
    }
}
