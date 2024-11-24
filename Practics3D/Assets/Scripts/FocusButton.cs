using UnityEngine;

public class FocusButtonController : MonoBehaviour
{
    public CameraController cameraController;
    public PrimitiveCreator primitiveCreator;

    public void FocusOnFirstObject()
    {
        Transform firstObject = primitiveCreator.GetFirstCreatedObject();
        if (firstObject != null)
        {
            cameraController.FocusOnObject(firstObject);
        }
    }
}
