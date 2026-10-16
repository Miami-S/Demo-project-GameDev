using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 6f;          // Скорость ходьбы
    public float gravity = -9.81f;    // Сила гравитации
    public float jumpHeight = 1.5f;   // Высота прыжка
    public float turnSmoothTime = 0.1f; // Плавность поворота

    Vector3 velocity;
    float turnSmoothVelocity;

    void Start()
    {
        // Автоматически находим Character Controller на игроке
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // 1. Проверяем, на земле ли игрок
        bool isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Сбрасываем падение
        }

        // 2. Получаем нажатия клавиш (WASD или стрелки)
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        // 3. Движение и поворот
        if (direction.magnitude >= 0.1f)
        {
            // Вычисляем угол поворота относительно камеры
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            // Поворачиваем игрока
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            // Двигаем игрока вперед
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }

        // 4. Прыжок
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // 5. Применяем гравитацию
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
