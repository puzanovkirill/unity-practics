using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine.SceneManagement;

public class ObjectSwitcher : MonoBehaviour
{
    public TMP_Dropdown objectDropdown; // Ссылка на Dropdown
    public Transform spawnPoint;    // Точка, где будут появляться объекты
    public List<ObjectData> objectDataList; // Список данных объектов
    private GameObject currentObject; // Текущий загруженный объект
    private ChildObjectSwitcher childObjectSwitcher;

    public GameObject nextChildButton;
    public GameObject previousChildButton;

    void Start()
    {
        LoadObjectData();
        LoadCustomObjects();
        PopulateDropdown();
        objectDropdown.onValueChanged.AddListener(delegate { SwitchObject(objectDropdown.value); });
        if (objectDataList.Count > 0)
        {
            SwitchObject(0); // Инициализация первого объекта
        }
        bool isChildModeActive = childObjectSwitcher.IsChildModeActive(); // Метод в ChildObjectSwitcher для получения состояния
        nextChildButton.SetActive(isChildModeActive);
        previousChildButton.SetActive(isChildModeActive);
    }
    void LoadObjectData()
    {
        // Загружаем все ObjectData из папки Resources/ObjectData
        ObjectData[] loadedData = Resources.LoadAll<ObjectData>("ObjectData");

        // Добавляем загруженные данные в список
        objectDataList.Clear();
        objectDataList.AddRange(loadedData);

        // Проверяем, были ли загружены данные
        if (objectDataList.Count == 0)
        {
            Debug.LogWarning("ObjectSwitcher: No ObjectData found in Resources/ObjectData.");
        }
    }
    void PopulateDropdown()
    {
        List<string> options = new List<string>();
        foreach (ObjectData data in objectDataList)
        {
            options.Add(data.objectName);
        }
        objectDropdown.ClearOptions();
        objectDropdown.AddOptions(options);
    }

    
    void SwitchObject(int index)
    {
        // Удаляем предыдущий объект
        if (currentObject != null)
        {
            Destroy(currentObject);
            childObjectSwitcher = null;
        }

        // Загружаем новый объект
        if (index >= 0 && index < objectDataList.Count)
        {
            ObjectData data = objectDataList[index];
            if (data.prefab == null)
            {
                // Если объект загружен из JSON, создаём его заново
                currentObject = CreateObjectFromData(data.objectName);
            }
            else
            {
                // Если объект уже существует как префаб, инстанцируем его
                currentObject = Instantiate(data.prefab, spawnPoint.position, Quaternion.identity);
            }

            // Проверяем и настраиваем ChildObjectSwitcher для нового объекта
            childObjectSwitcher = currentObject.GetComponent<ChildObjectSwitcher>();
            if (childObjectSwitcher == null && currentObject.transform.childCount > 0)
            {
                childObjectSwitcher = currentObject.AddComponent<ChildObjectSwitcher>();
            }

            // Обновляем информацию об объекте
            FindObjectOfType<ObjectInfoDisplay>().UpdateInfo(data);

            // Обновляем цель камеры
            Camera.main.GetComponent<OrbitCamera>().target = currentObject.transform;

            Camera.main.GetComponent<OrbitCamera>().ResetCameraPosition();

            Camera.main.GetComponent<OrbitCamera>().AdjustCameraDistance();

            FindObjectOfType<ObjectAnimator>()?.UpdateButtonState(); // Убираем кнопку активации, если нет доступных анимаций.
        }
    }
    GameObject CreateObjectFromData(string objectName)
    {
        string filePath = Path.Combine(Application.persistentDataPath, objectName + ".json");
        if (!File.Exists(filePath))
        {
            Debug.LogWarning($"File not found for object: {objectName}");
            return null;
        }

        // Загружаем данные из файла
        string json = File.ReadAllText(filePath);
        SerializableObjectData data = JsonUtility.FromJson<SerializableObjectData>(json);

        // Создаём объект
        GameObject customObject = new GameObject(data.objectName);
        foreach (SerializablePrimitive primitiveData in data.primitives)
        {
            PrimitiveType type = (PrimitiveType)System.Enum.Parse(typeof(PrimitiveType), primitiveData.type);
            GameObject primitive = GameObject.CreatePrimitive(type);
            primitive.transform.SetParent(customObject.transform);
            primitive.transform.localPosition = primitiveData.position;
            primitive.transform.localEulerAngles = primitiveData.rotation;
            primitive.transform.localScale = primitiveData.scale;
        }

        customObject.transform.position = spawnPoint.position;
        return customObject;
    }

    public void ToggleChildMode()
    {
        if (childObjectSwitcher != null)
        {
            childObjectSwitcher.ToggleChildMode();
            // Управляем видимостью кнопок в зависимости от состояния
            bool isChildModeActive = childObjectSwitcher.IsChildModeActive(); // Метод в ChildObjectSwitcher для получения состояния
            nextChildButton.SetActive(isChildModeActive);
            previousChildButton.SetActive(isChildModeActive);

            // Центрируем камеру на выбранном объекте
            CenterCameraOnTarget();
        }
        else
        {
            Debug.LogWarning("ObjectSwitcher: Current object has no child objects to iterate.");
        }
    }

    public void NextChild()
    {
        if (childObjectSwitcher != null)
        {
            childObjectSwitcher.NextChild();
            CenterCameraOnTarget();
        }
        else
        {
            Debug.LogWarning("ObjectSwitcher: Current object has no child objects to iterate.");
        }
    }

    public void PreviousChild()
    {
        if (childObjectSwitcher != null)
        {
            childObjectSwitcher.PreviousChild();
            CenterCameraOnTarget();
        }
        else
        {
            Debug.LogWarning("ObjectSwitcher: Current object has no child objects to iterate.");
        }
    }
    private void CenterCameraOnTarget()
    {
        if (childObjectSwitcher != null && childObjectSwitcher.IsChildModeActive())
        {
            // В режиме перебора дочерних объектов центрируем камеру на текущем дочернем объекте
            Transform currentChild = childObjectSwitcher.GetCurrentChild();
            if (currentChild != null)
            {
                Camera.main.GetComponent<OrbitCamera>().target = currentChild;
                FindObjectOfType<ObjectInfoDisplay>()?.UpdateName(currentChild.name);
            }
        }
        else
        {
            // Если режим перебора отключён, центрируем камеру на корневом объекте
            if (currentObject != null)
            {
                Camera.main.GetComponent<OrbitCamera>().target = currentObject.transform;
                FindObjectOfType<ObjectInfoDisplay>()?.UpdateName(currentObject.name);
            }
        }
    }

    public void SliceCurrentObject()
    {
        GameObject targetObject = null;

        if (childObjectSwitcher != null && childObjectSwitcher.IsChildModeActive())
        {
            Transform currentChild = childObjectSwitcher.GetCurrentChild();
            if (currentChild != null)
            {
                targetObject = currentChild.gameObject;
            }
        }
        else
        {
            if (currentObject != null)
            {
                targetObject = currentObject;
            }
        }

        if (targetObject != null)
        {
            ObjectSlicer slicer = targetObject.GetComponent<ObjectSlicer>();
            if (slicer == null)
            {
                slicer = targetObject.AddComponent<ObjectSlicer>();
            }

            slicer.SliceObject();
        }
        else
        {
            Debug.LogWarning("ObjectSwitcher: No object to slice.");
        }
    }
    void LoadCustomObjects()
    {
        string[] files = Directory.GetFiles(Application.persistentDataPath, "*.json");
        foreach (string file in files)
        {
            string json = File.ReadAllText(file);
            SerializableObjectData data = JsonUtility.FromJson<SerializableObjectData>(json);

            // Создаём ObjectData для хранения информации
            ObjectData objectData = ScriptableObject.CreateInstance<ObjectData>();
            objectData.objectName = data.objectName;
            objectData.description = data.description;

            // Временно сохраняем данные объекта для восстановления при выборе
            objectData.prefab = null; // prefab создаётся динамически при выборе
            objectDataList.Add(objectData);
        }

        Debug.Log($"Loaded {objectDataList.Count} custom objects.");
    }
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}

