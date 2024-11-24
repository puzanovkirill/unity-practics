using UnityEngine;

public class ChildObjectSwitcher : MonoBehaviour
{
    private Transform[] childObjects; // Массив дочерних объектов
    private int currentChildIndex = -1; // Индекс текущего дочернего объекта (-1 = полный объект)
    private bool isChildMode = false; // Режим перебора дочерних объектов

    void Start()
    {
        // Получаем все дочерние объекты
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
        // Входим в режим просмотра дочерних объектов
        currentChildIndex = 0;
        UpdateChildVisibility();
    }

    private void ExitChildMode()
    {
        // Выходим из режима просмотра дочерних объектов (показываем объект целиком)
        currentChildIndex = -1;
        UpdateChildVisibility();
    }

    public void NextChild()
    {
        if (!isChildMode || childObjects == null) return;

        currentChildIndex = (currentChildIndex + 1) % childObjects.Length; // Переход к следующему
        UpdateChildVisibility();
    }

    public void PreviousChild()
    {
        if (!isChildMode || childObjects == null) return;

        currentChildIndex = (currentChildIndex - 1 + childObjects.Length) % childObjects.Length; // Переход к предыдущему
        UpdateChildVisibility();
    }

    private void UpdateChildVisibility()
    {
        if (childObjects == null) return;

        if (currentChildIndex == -1)
        {
            // Показываем весь объект целиком
            foreach (Transform child in childObjects)
            {
                child.gameObject.SetActive(true);
            }
        }
        else
        {
            // Показываем только текущий дочерний объект
            for (int i = 0; i < childObjects.Length; i++)
            {
                childObjects[i].gameObject.SetActive(i == currentChildIndex);
            }
        }
    }
    // Метод для получения текущего состояния режима дочерних объектов
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
