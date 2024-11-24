using UnityEngine;
using UnityEngine.UI;

public class ObjectAnimator : MonoBehaviour
{
    public OrbitCamera orbitCamera;
    private bool isAnimationActive = false;
    public string animationBoolParameter = "IsRotating";
    public GameObject activateButton;

    public void ToggleAnimation()
    {
        // ���������, ���� �� ������� ���� � ������
        if (orbitCamera.target == null)
        {
            Debug.LogWarning("DynamicAnimationController: No target");
            return;
        }

        // �������� �������� Animator � �������� �������
        Animator targetAnimator = orbitCamera.target.GetComponent<Animator>();
        if (targetAnimator == null)
        {
            Debug.LogWarning("DynamicAnimationController: No animator.");
            return;
        }

        // ����������� ��������� ��������
        isAnimationActive = !isAnimationActive;

        // ������������� �������� �������� ��������� � Animator
        targetAnimator.SetBool(animationBoolParameter, isAnimationActive);
    }
    public void UpdateButtonState()
    {
        // ���������, ���� �� ������� ���� � ������
        if (orbitCamera.target == null)
        {
            activateButton.SetActive(false); // �������� ������
            return;
        }

        // ��������� ������� Animator � �������� �������
        Animator targetAnimator = orbitCamera.target.GetComponent<Animator>();
        activateButton.SetActive(targetAnimator != null); // ���������� ������, ������ ���� Animator ����������
    }
}