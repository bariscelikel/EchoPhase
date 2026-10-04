using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 6f;
    [SerializeField] private float rotationSpeed = 10f;

    [Header("Camera")]
    [SerializeField] private Transform cameraTransform;

    [Header("Animation")]
    [SerializeField] private Animator animator;

    [Header("Echo")]
    private EchoRecorder echoRecorder;

    private Rigidbody rb;
    private Vector2 moveInput;
    private bool jumpPressed;
    private bool isGrounded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        echoRecorder = GetComponent<EchoRecorder>();

        if (animator == null)
        {
            Transform robot = transform.Find("ModularRobots");

            if (robot != null)
            {
                animator = robot.GetComponent<Animator>();
            }
        }
    }

    private void Start()
    {
        isGrounded = true;
    }

    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    private void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            jumpPressed = true;
        }
    }

    private void OnEcho(InputValue value)
    {
        if (!value.isPressed)
            return;

        if (echoRecorder == null)
            return;

        if (echoRecorder.IsRecording())
        {
            echoRecorder.StopRecording();
        }
        else
        {
            echoRecorder.StartRecording();
        }
    }

    private void FixedUpdate()
    {
        // Kamera yönüne göre hareket
        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;

        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 movement =
            cameraForward * moveInput.y +
            cameraRight * moveInput.x;

        // Animasyon
        if (animator != null)
        {
            animator.SetFloat("Speed", movement.magnitude);
        }

        // Hareket
        Vector3 newVelocity = new Vector3(
            movement.x * moveSpeed,
            rb.linearVelocity.y,
            movement.z * moveSpeed
        );

        rb.linearVelocity = newVelocity;

        // Karakteri hareket yönüne döndür
        if (movement.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation =
                Quaternion.LookRotation(movement);

            Quaternion smoothRotation =
                Quaternion.Slerp(
                    rb.rotation,
                    targetRotation,
                    rotationSpeed * Time.fixedDeltaTime
                );

            rb.MoveRotation(smoothRotation);
        }

        // Zıplama
        if (jumpPressed && isGrounded)
        {
            rb.AddForce(
                Vector3.up * jumpForce,
                ForceMode.Impulse
            );

            isGrounded = false;
        }

        jumpPressed = false;
    }

    private void OnCollisionStay(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            // Sadece üst yüzeye basıyorsak yerdeyiz
            if (contact.normal.y > 0.5f)
            {
                isGrounded = true;
                return;
            }
        }
    }
}