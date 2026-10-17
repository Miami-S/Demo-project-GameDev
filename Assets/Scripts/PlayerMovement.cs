using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Ссылки")]
    [SerializeField] private CharacterController controller;

    [Header("Движение")]
    [SerializeField] private float walkSpeed = 6f;
    [SerializeField] private float turnSmoothTime = 0.1f;

    [Header("Прыжок и гравитация")]
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float groundedStick = -2f;

    private Transform cameraTransform;
    private Vector3 verticalVelocity;
    private float turnSmoothVelocity;

    private void Awake()
    {
        if (controller == null)
            controller = GetComponent<CharacterController>();

        if (Camera.main != null)
            cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        HandleGravity();
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

    private void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 inputDirection = new Vector3(horizontal, 0f, vertical).normalized;

        if (inputDirection.magnitude < 0.1f)
            return;

        float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg
                            + cameraTransform.eulerAngles.y;

        float smoothedAngle = Mathf.SmoothDampAngle(
            transform.eulerAngles.y,
            targetAngle,
            ref turnSmoothVelocity,
            turnSmoothTime);

        transform.rotation = Quaternion.Euler(0f, smoothedAngle, 0f);

        Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        controller.Move(moveDirection.normalized * walkSpeed * Time.deltaTime);
    }

    private void HandleJump()
    {
        if (!Input.GetButtonDown("Jump") || !controller.isGrounded)
            return;

        verticalVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }
}
