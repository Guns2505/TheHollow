using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    public Transform cameraPivot;
    public float minPitch = -85f;
    public float maxPitch = 85f;
    public float headBobAmount = 0.045f;
    public float headBobSpeed = 10f;

    PlayerController controller;
    Vector3 cameraStartPosition;
    float pitch;
    float bobTimer;

    void Awake()
    {
        controller = GetComponent<PlayerController>();
        cameraStartPosition = cameraPivot.localPosition;
    }

    void Update()
    {
        if (!GameManager.PlayerInputEnabled || Mouse.current == null) return;

        Vector2 delta = Mouse.current.delta.ReadValue() * GameSettings.MouseSensitivity * 0.05f;

        transform.Rotate(Vector3.up * delta.x);
        pitch = Mathf.Clamp(pitch - delta.y, minPitch, maxPitch);
        cameraPivot.localRotation = Quaternion.Euler(pitch, 0f, 0f);

        ApplyHeadBob();
    }

    void ApplyHeadBob()
    {
        if (controller.IsMoving && controller.IsGrounded)
            bobTimer += Time.deltaTime * headBobSpeed * (controller.IsDashing ? 2f : 1f);
        else
            bobTimer = 0f;

        Vector3 offset = new Vector3(Mathf.Cos(bobTimer * 0.5f) * headBobAmount * 0.5f, Mathf.Sin(bobTimer) * headBobAmount, 0f);
        cameraPivot.localPosition = Vector3.Lerp(cameraPivot.localPosition, cameraStartPosition + offset, Time.deltaTime * 12f);
    }
}
