using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewObjectData", menuName = "Object Data")]
public class ObjectData : ScriptableObject
{
    public string objectName;
    [TextArea]
    public string description;
    public GameObject prefab;
    public List<SerializablePrimitive> primitives;
}
