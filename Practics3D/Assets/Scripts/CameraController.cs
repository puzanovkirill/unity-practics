using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float rotationSpeed = 5.0f;   // Скорость вращения камеры (ПКМ)
    public float panSpeed = 10.0f;       // Скорость сдвига камеры (СКМ)
    public float zoomSpeed = 50.0f;      // Скорость зума (колесо мыши)
    public float minZoom = 2.0f;         // Минимальная высота камеры
    public float maxZoom = 100.0f;       // Максимальная высота камеры

    private Vector3 lastMousePosition;   // Последняя позиция мыши
    private Transform cameraTransform;   // Трансформ камеры

    void Start()
    {
        cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        HandleMouseInput();
    }

    void HandleMouseInput()
    {
        // ПКМ: Вращение камеры
        if (Input.GetMouseButton(1))
        {
            RotateCamera();
        }

        // СКМ: Сдвиг камеры
        if (Input.GetMouseButton(2))
        {
            PanCamera();
        }

        // Колесо мыши: Зум
        ZoomCamera();
    }

    void RotateCamera()
    {
        // Разница позиций мыши
        Vector3 delta = Input.mousePosition - lastMousePosition;

        // Вычисляем углы вращения
        float yaw = delta.x * rotationSpeed * Time.deltaTime;
        float pitch = -delta.y * rotationSpeed * Time.deltaTime;

        // Вращаем камеру относительно её собственной позиции
        cameraTransform.RotateAround(cameraTransform.position, Vector3.up, yaw);
        cameraTransform.RotateAround(cameraTransform.position, cameraTransform.right, pitch);

    }

    void PanCamera()
    {
        // Разница позиций мыши
        Vector3 delta = Input.mousePosition - lastMousePosition;

        // Локальные направления движения
        Vector3 right = cameraTransform.right;
        Vector3 up = cameraTransform.up;

        // Вычисляем движение
        Vector3 movement = -right * delta.x * panSpeed * Time.deltaTime + -up * delta.y * panSpeed * Time.deltaTime;

        // Перемещаем камеру
        cameraTransform.position += movement;
    }

    void ZoomCamera()
    {

        float scroll = Input.GetAxis("Mouse ScrollWheel");

        Vector3 direction = cameraTransform.forward;
        cameraTransform.position += direction * scroll * zoomSpeed * Time.deltaTime;

    }

    void LateUpdate()
    {
        // Сохраняем текущую позицию мыши
        lastMousePosition = Input.mousePosition;
    }

    public void FocusOnObject(Transform target)
    {
        if (target == null)
        {
            Debug.LogWarning("CameraController: Target is null. Cannot focus.");
            return;
        }

        // Сместить камеру немного назад относительно объекта
        Vector3 focusPosition = target.position - cameraTransform.forward * 10.0f;

        // Плавное перемещение камеры к объекту
        cameraTransform.position = focusPosition;

        // Поворачиваем камеру, чтобы она смотрела на объект
        cameraTransform.LookAt(target);
    }
}
