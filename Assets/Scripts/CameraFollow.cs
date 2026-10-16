using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;            // Ссылка на игрока
    public float distance = 5f;         // Дистанция от камеры до игрока
    public float sensitivity = 3f;      // Чувствительность мыши
    public float height = 2f;           // Высота камеры над игроком

    public float minY = -20f;           // Максимальный угол взгляда вниз
    public float maxY = 60f;            // Максимальный угол взгляда вверх

    private float currentX = 0f;
    private float currentY = 0f;

    void Start()
    {
        // Прячем и блокируем курсор мыши в центре экрана
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // 1. Получаем движение мыши
        currentX += Input.GetAxis("Mouse X") * sensitivity;
        currentY -= Input.GetAxis("Mouse Y") * sensitivity;

        // 2. Ограничиваем взгляд по вертикали, чтобы камера не переворачивалась
        currentY = Mathf.Clamp(currentY, minY, maxY);

        // 3. Вычисляем новое положение и поворот камеры
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        Vector3 position = target.position - (rotation * Vector3.forward * distance) + (Vector3.up * height);

        // 4. Применяем позицию и заставляем камеру смотреть на игрока
        transform.position = position;
        transform.LookAt(target.position + Vector3.up * 1.5f); // Смотрим чуть выше ног игрока
    }
}