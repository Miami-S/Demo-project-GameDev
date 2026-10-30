using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Цель")]
    [SerializeField] private Transform target;
    [SerializeField] private float targetLookHeight = 1.5f;

    [Header("Позиция камеры")]
    [SerializeField] private float distance = 5f;
    [SerializeField] private float height = 2f;
    [SerializeField] private float shoulderOffset = 0.7f;

    [Header("Управление мышью")]
    [SerializeField] private float sensitivity = 3f;
    [SerializeField] private float minPitch = -20f;
    [SerializeField] private float maxPitch = 60f;

    [Header("Коллизия камеры")] // НОВОЕ
    [SerializeField] private float cameraRadius = 0.3f;
    [SerializeField] private LayerMask collisionMask;
    [SerializeField] private float minDistance = 1f;

    private float yaw;
    private float pitch;

    private void Start()
    {
        LockCursor();
    }

    private void LateUpdate()
    {
        if (Time.timeScale == 0f) return;

        if (target == null)
            return;

        ReadMouseInput();
        ApplyCameraTransform();
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void ReadMouseInput()
    {
        yaw += Input.GetAxis("Mouse X") * sensitivity;
        pitch -= Input.GetAxis("Mouse Y") * sensitivity;

        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
    }

    private void ApplyCameraTransform()
    {
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);

        // 1. Вычисляем точку, на которую смотрит камера (голова игрока + сдвиг влево)
        Vector3 lateralOffset = rotation * Vector3.left * shoulderOffset;
        Vector3 lookTarget = target.position + Vector3.up * targetLookHeight + lateralOffset;

        // 2. Вычисляем идеальную позицию камеры (как было в оригинале)
        Vector3 desiredPosition = target.position
                                - rotation * Vector3.forward * distance
                                + Vector3.up * height
                                + lateralOffset;

        // 3. Вычисляем расстояние и направление от головы игрока к идеальной камере
        Vector3 direction = (desiredPosition - lookTarget).normalized;
        float desiredDistance = Vector3.Distance(lookTarget, desiredPosition);

        float finalDistance = desiredDistance;

        // 4. Пускаем луч с толщиной (SphereCast) от головы игрока в сторону камеры
        RaycastHit hit;
        if (Physics.SphereCast(lookTarget, cameraRadius, direction, out hit, desiredDistance, collisionMask))
        {
            // Если врезались в стену, ставим камеру прямо перед ней
            finalDistance = hit.distance;

            // Не даем камере прилипнуть вплотную к лицу игрока
            if (finalDistance < minDistance)
                finalDistance = minDistance;
        }

        // 5. Применяем финальную позицию
        transform.position = lookTarget + direction * finalDistance;
        transform.LookAt(lookTarget);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
