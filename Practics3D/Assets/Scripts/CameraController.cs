using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float rotationSpeed = 5.0f;   // �������� �������� ������ (���)
    public float panSpeed = 10.0f;       // �������� ������ ������ (���)
    public float zoomSpeed = 50.0f;      // �������� ���� (������ ����)
    public float minZoom = 2.0f;         // ����������� ������ ������
    public float maxZoom = 100.0f;       // ������������ ������ ������

    private Vector3 lastMousePosition;   // ��������� ������� ����
    private Transform cameraTransform;   // ��������� ������

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
        // ���: �������� ������
        if (Input.GetMouseButton(1))
        {
            RotateCamera();
        }

        // ���: ����� ������
        if (Input.GetMouseButton(2))
        {
            PanCamera();
        }

        // ������ ����: ���
        ZoomCamera();
    }

    void RotateCamera()
    {
        // ������� ������� ����
        Vector3 delta = Input.mousePosition - lastMousePosition;

        // ��������� ���� ��������
        float yaw = delta.x * rotationSpeed * Time.deltaTime;
        float pitch = -delta.y * rotationSpeed * Time.deltaTime;

        // ������� ������ ������������ � ����������� �������
        cameraTransform.RotateAround(cameraTransform.position, Vector3.up, yaw);
        cameraTransform.RotateAround(cameraTransform.position, cameraTransform.right, pitch);

    }

    void PanCamera()
    {
        // ������� ������� ����
        Vector3 delta = Input.mousePosition - lastMousePosition;

        // ��������� ����������� ��������
        Vector3 right = cameraTransform.right;
        Vector3 up = cameraTransform.up;

        // ��������� ��������
        Vector3 movement = -right * delta.x * panSpeed * Time.deltaTime + -up * delta.y * panSpeed * Time.deltaTime;

        // ���������� ������
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
        // ��������� ������� ������� ����
        lastMousePosition = Input.mousePosition;
    }

    public void FocusOnObject(Transform target)
    {
        if (target == null)
        {
            Debug.LogWarning("CameraController: Target is null. Cannot focus.");
            return;
        }

        // �������� ������ ������� ����� ������������ �������
        Vector3 focusPosition = target.position - cameraTransform.forward * 10.0f;

        // ������� ����������� ������ � �������
        cameraTransform.position = focusPosition;

        // ������������ ������, ����� ��� �������� �� ������
        cameraTransform.LookAt(target);
    }
}
