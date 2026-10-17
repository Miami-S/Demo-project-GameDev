using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Цель")]
    [SerializeField] private Transform target;
    [SerializeField] private float targetLookHeight = 1.5f;

    [Header("Позиция камеры")]
    [SerializeField] private float distance = 5f;
    [SerializeField] private float height = 2f;

    [Header("Управление мышью")]
    [SerializeField] private float sensitivity = 3f;
    [SerializeField] private float minPitch = -20f;
    [SerializeField] private float maxPitch = 60f;

    private float yaw;
    private float pitch;

    private void Start()
    {
        LockCursor();
    }

    private void LateUpdate()
    {
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

        Vector3 position = target.position
                         - rotation * Vector3.forward * distance
                         + Vector3.up * height;

        transform.position = position;
        transform.LookAt(target.position + Vector3.up * targetLookHeight);
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