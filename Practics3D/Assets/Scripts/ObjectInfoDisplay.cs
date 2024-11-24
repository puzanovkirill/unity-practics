using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ObjectInfoDisplay : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text descriptionText;

    public void UpdateInfo(ObjectData data)
    {
        nameText.text = data.objectName;
        descriptionText.text = data.description;
    }
    public void UpdateName(string name)
    {
        if (nameText != null)
        {
            nameText.text = name;
        }
    }
}