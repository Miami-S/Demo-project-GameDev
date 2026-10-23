using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Ссылки")]
    [SerializeField] private CharacterController controller;
    [SerializeField] private Transform cameraTransform;

    [Header("Движение")]
    [SerializeField] private float walkSpeed = 6f;
    [SerializeField] private float turnSmoothTime = 0.1f;

    [Header("Прыжок и гравитация")]
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float groundedStick = -2f;

    private Vector3 verticalVelocity;
    private float turnSmoothVelocity;

    private void Awake()
    {
        if (controller == null)
            controller = GetComponent<CharacterController>();

        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        HandleGravity();
        HandleRotation();
        HandleMovement();
        HandleJump();
    }

    private void HandleGravity()
    {
        bool isGrounded = controller.isGrounded;

        if (isGrounded && verticalVelocity.y < 0f)
            verticalVelocity.y = groundedStick;

        verticalVelocity.y += gravity * Time.deltaTime;
        controller.Move(verticalVelocity * Time.deltaTime);
    }

    private void HandleRotation()
    {
        if (cameraTransform == null)
            return;

        float targetAngle = cameraTransform.eulerAngles.y;

        float smoothedAngle = Mathf.SmoothDampAngle(
            transform.eulerAngles.y,
            targetAngle,
            ref turnSmoothVelocity,
            turnSmoothTime);

        transform.rotation = Quaternion.Euler(0f, smoothedAngle, 0f);
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 inputDirection = new Vector3(horizontal, 0f, vertical);
        if (inputDirection.magnitude < 0.1f)
            return;

        Vector3 moveDirection;

        if (cameraTransform != null)
        {
            Vector3 camForward = cameraTransform.forward;
            Vector3 camRight = cameraTransform.right;
            camForward.y = 0f;
            camRight.y = 0f;
            camForward.Normalize();
            camRight.Normalize();

            moveDirection = camForward * vertical + camRight * horizontal;
        }
        else
        {
            moveDirection = Quaternion.Euler(0f, transform.eulerAngles.y, 0f) * inputDirection;
        }

        if (moveDirection.magnitude > 1f)
            moveDirection.Normalize();

        controller.Move(moveDirection * walkSpeed * Time.deltaTime);
    }

    private void HandleJump()
    {
        if (!Input.GetButtonDown("Jump") || !controller.isGrounded)
            return;

        verticalVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }
}
