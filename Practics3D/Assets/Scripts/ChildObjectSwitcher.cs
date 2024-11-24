using UnityEngine;

public class ChildObjectSwitcher : MonoBehaviour
{
    private Transform[] childObjects; // ������ �������� ��������
    private int currentChildIndex = -1; // ������ �������� ��������� ������� (-1 = ������ ������)
    private bool isChildMode = false; // ����� �������� �������� ��������

    void Start()
    {
        // �������� ��� �������� �������
        int childCount = transform.childCount;
        if (childCount > 0)
        {
            childObjects = new Transform[childCount];
            for (int i = 0; i < childCount; i++)
            {
                childObjects[i] = transform.GetChild(i);
            }
        }
        else
        {
            childObjects = null;
        }
    }

    public void ToggleChildMode()
    {
        if (childObjects == null || childObjects.Length == 0)
        {
            Debug.LogWarning("No child objects to iterate!");
            return;
        }

        isChildMode = !isChildMode;

        if (isChildMode)
        {
            EnterChildMode();
        }
        else
        {
            ExitChildMode();
        }
    }

    private void EnterChildMode()
    {
        // ������ � ����� ��������� �������� ��������
        currentChildIndex = 0;
        UpdateChildVisibility();
    }

    private void ExitChildMode()
    {
        // ������� �� ������ ��������� �������� �������� (���������� ������ �������)
        currentChildIndex = -1;
        UpdateChildVisibility();
    }

    public void NextChild()
    {
        if (!isChildMode || childObjects == null) return;

        currentChildIndex = (currentChildIndex + 1) % childObjects.Length; // ������� � ����������
        UpdateChildVisibility();
    }

    public void PreviousChild()
    {
        if (!isChildMode || childObjects == null) return;

        currentChildIndex = (currentChildIndex - 1 + childObjects.Length) % childObjects.Length; // ������� � �����������
        UpdateChildVisibility();
    }

    private void UpdateChildVisibility()
    {
        if (childObjects == null) return;

        if (currentChildIndex == -1)
        {
            // ���������� ���� ������ �������
            foreach (Transform child in childObjects)
            {
                child.gameObject.SetActive(true);
            }
        }
        else
        {
            // ���������� ������ ������� �������� ������
            for (int i = 0; i < childObjects.Length; i++)
            {
                childObjects[i].gameObject.SetActive(i == currentChildIndex);
            }
        }
    }
    // ����� ��� ��������� �������� ��������� ������ �������� ��������
    public bool IsChildModeActive()
    {
        return isChildMode;
    }

    public Transform GetCurrentChild()
    {
        if (isChildMode && childObjects != null && currentChildIndex >= 0 && currentChildIndex < childObjects.Length)
        {
            return childObjects[currentChildIndex];
        }
        return null;
    }
}
