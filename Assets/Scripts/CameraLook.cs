using UnityEngine;
using UnityEngine.InputSystem;

public class CameraLook : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Camera Settings")]
    [SerializeField] private float mouseSensitivity = 0.15f;
    [SerializeField] private float distance = 7f;
    [SerializeField] private float height = 4.5f;

    [Header("Vertical Limits")]
    [SerializeField] private float minPitch = -30f;
    [SerializeField] private float maxPitch = 60f;

    [Header("Camera Collision")]
    [SerializeField] private LayerMask collisionMask;
    [SerializeField] private float collisionRadius = 0.2f;
    [SerializeField] private float collisionOffset = 0.3f;

    private float yaw;
    private float pitch = 15f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void LateUpdate()
    {
        if (target == null)
            return;

        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        yaw += mouseDelta.x * mouseSensitivity;
        pitch -= mouseDelta.y * mouseSensitivity;

        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        Quaternion rotation =
            Quaternion.Euler(pitch, yaw, 0f);

        Vector3 targetPosition =
            target.position + Vector3.up * height;

        Vector3 desiredOffset =
            rotation * new Vector3(0f, 0f, -distance);

        Vector3 desiredPosition =
            targetPosition + desiredOffset;

        Vector3 direction =
            desiredPosition - targetPosition;

        float desiredDistance = direction.magnitude;

        if (Physics.SphereCast(
            targetPosition,
            collisionRadius,
            direction.normalized,
            out RaycastHit hit,
            desiredDistance,
            collisionMask))
        {
            desiredDistance =
                Mathf.Max(0.5f, hit.distance - collisionOffset);
        }

        transform.position =
            targetPosition +
            direction.normalized * desiredDistance;

        transform.LookAt(targetPosition);
    }
}