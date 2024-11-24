using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectMover : MonoBehaviour
{
    private Camera mainCamera;
    private GameObject selectedObject;
    private Vector3 offset;
    private bool isDragging = false;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        // ��������� ������� ����
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            // ��� �� ������ � ����� �����
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                // �������� ������
                selectedObject = hit.collider.gameObject;
                offset = selectedObject.transform.position - hit.point;
                isDragging = true;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            selectedObject = null;
        }

        if (isDragging && selectedObject != null)
        {
            // ���������� ������
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            float distance;
            if (plane.Raycast(ray, out distance))
            {
                Vector3 point = ray.GetPoint(distance) + offset;
                selectedObject.transform.position = point;
            }
        }
    }
}
