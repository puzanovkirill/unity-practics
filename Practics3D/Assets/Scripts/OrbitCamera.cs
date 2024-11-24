using UnityEngine;

public class OrbitCamera : MonoBehaviour
{
    public Transform target; // Цель, вокруг которой вращается камера
    public float distance = 5.0f;

    public float xSpeed = 120.0f;
    public float ySpeed = 120.0f;

    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;

    public float zoomSpeed = 4.0f;
    public float minDistance = 1.0f;
    public float maxDistance = 30.0f;

    private float x = 0.0f;
    private float y = 0.0f;

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

    }

    void LateUpdate()
    {
        if (target)
        {
            if (Input.GetMouseButton(0)) // Зажата ЛКМ
            {
                x += Input.GetAxis("Mouse X") * xSpeed * 10 * Time.deltaTime;
                y -= Input.GetAxis("Mouse Y") * ySpeed * 10 * Time.deltaTime;

                y = Mathf.Clamp(y, yMinLimit, yMaxLimit);
            }
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            distance -= scroll * zoomSpeed;
            distance = Mathf.Clamp(distance, minDistance, maxDistance);

            Quaternion rotation = Quaternion.Euler(y, x, 0);
            Vector3 position = rotation * new Vector3(0.0f, 0.0f, -distance) + target.position;

            transform.rotation = rotation;
            transform.position = position;
        }
    }
    public void AdjustCameraDistance()
    {
        Renderer objectRenderer = target.GetComponent<Renderer>();
        if (objectRenderer != null)
        {
            Bounds bounds = objectRenderer.bounds;
            distance = bounds.size.magnitude * 1.5f; // Увеличиваем дистанцию пропорционально размеру объекта
        }
    }
    public void ResetCameraPosition()
    {
        x = 0.0f;
        y = 0.0f;
    }
}
