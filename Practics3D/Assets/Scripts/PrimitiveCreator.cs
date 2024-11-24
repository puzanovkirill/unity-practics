using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PrimitiveCreator : MonoBehaviour
{
    public Transform creationArea; // Область, где будут появляться объекты
    public GameObject currentObject; // Текущий редактируемый объект
    private List<GameObject> createdObjects = new List<GameObject>(); // Список созданных объектов

    // Методы для создания примитивов
    public void CreateCube()
    {
        CreatePrimitive(PrimitiveType.Cube);
    }

    public void CreateSphere()
    {
        CreatePrimitive(PrimitiveType.Sphere);
    }

    public void CreateCylinder()
    {
        CreatePrimitive(PrimitiveType.Cylinder);
    }

    public void CreateCapsule()
    {
        CreatePrimitive(PrimitiveType.Capsule);
    }

    public void CreatePlane()
    {
        CreatePrimitive(PrimitiveType.Plane);
    }

    private void CreatePrimitive(PrimitiveType type)
    {
        GameObject primitive = GameObject.CreatePrimitive(type);
        primitive.transform.SetParent(creationArea, false);
        primitive.transform.localPosition = Vector3.zero;
        primitive.name = type.ToString();
        createdObjects.Add(primitive);
        currentObject = primitive;
    }

    // Метод для сохранения объекта
    public void SaveObject()
    {
        if (createdObjects.Count == 0)
        {
            Debug.LogWarning("No objects to save!");
            return;
        }

        // Создаём пустой объект для хранения всех примитивов
        GameObject savedObject = new GameObject("CustomObject");
        foreach (GameObject obj in createdObjects)
        {
            obj.transform.SetParent(savedObject.transform);
        }

        // Создаём данные объекта
        SerializableObjectData data = new SerializableObjectData
        {
            objectName = "CustomObject",
            description = "Custom created object",
            primitives = new List<SerializablePrimitive>() // Инициализация списка
        };

        // Добавляем данные каждого примитива
        foreach (Transform child in savedObject.transform)
        {
            SerializablePrimitive primitive = new SerializablePrimitive
            {
                type = child.name,
                position = child.localPosition,
                rotation = child.localEulerAngles,
                scale = child.localScale
            };
            data.primitives.Add(primitive); // Добавляем примитив в список
        }

        // Сохраняем данные в JSON
        string json = JsonUtility.ToJson(data, true);
        string filePath = System.IO.Path.Combine(Application.persistentDataPath, data.objectName + ".json");
        System.IO.File.WriteAllText(filePath, json);

        // Удаляем временный объект с примитивами
        Destroy(savedObject);

        // Очищаем список созданных объектов
        createdObjects.Clear();

        Debug.Log($"Object saved successfully! File path: {filePath}");
    }



    private void SaveObjectData(ObjectData data)
    {
        // Создаём структуру для сохранения данных
        SerializableObjectData serializableData = new SerializableObjectData(data);
        string json = JsonUtility.ToJson(serializableData, true);

        // Сохраняем JSON в файл
        string filePath = System.IO.Path.Combine(Application.persistentDataPath, data.objectName + ".json");
        System.IO.File.WriteAllText(filePath, json);

        // Дополнительно можно сохранить объект в список загруженных объектов
        // или обновить Dropdown на сцене просмотра
    }
    // Метод для получения первого объекта
    public Transform GetFirstCreatedObject()
    {
        if (createdObjects.Count > 0)
        {
            return createdObjects[0].transform;
        }

        Debug.LogWarning("PrimitiveCreator: No objects have been created.");
        return null;
    }
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}

[System.Serializable]
public class SerializableObjectData
{
    public string objectName;
    public string description;
    public List<SerializablePrimitive> primitives = new List<SerializablePrimitive>();

    // Конструктор без параметров
    public SerializableObjectData() { }

    // Конструктор с параметром ObjectData
    public SerializableObjectData(ObjectData data)
    {
        objectName = data.objectName;
        description = data.description;

        foreach (Transform child in data.prefab.transform)
        {
            MeshFilter meshFilter = child.GetComponent<MeshFilter>();
            if (meshFilter != null)
            {
                SerializablePrimitive primitive = new SerializablePrimitive
                {
                    type = child.name,
                    position = child.localPosition,
                    rotation = child.localEulerAngles,
                    scale = child.localScale
                };
                primitives.Add(primitive);
            }
        }
    }
}

[System.Serializable]
public class SerializablePrimitive
{
    public string type;
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 scale;
}
