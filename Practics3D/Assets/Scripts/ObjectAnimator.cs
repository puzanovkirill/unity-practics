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
        // Проверяем, есть ли текущая цель у камеры
        if (orbitCamera.target == null)
        {
            Debug.LogWarning("DynamicAnimationController: No target");
            return;
        }

        // Пытаемся получить Animator у текущего объекта
        Animator targetAnimator = orbitCamera.target.GetComponent<Animator>();
        if (targetAnimator == null)
        {
            Debug.LogWarning("DynamicAnimationController: No animator.");
            return;
        }

        // Переключаем состояние анимации
        isAnimationActive = !isAnimationActive;

        // Устанавливаем значение булевого параметра в Animator
        targetAnimator.SetBool(animationBoolParameter, isAnimationActive);
    }
    public void UpdateButtonState()
    {
        // Проверяем, есть ли текущая цель у камеры
        if (orbitCamera.target == null)
        {
            activateButton.SetActive(false); // Скрываем кнопку
            return;
        }

        // Проверяем наличие Animator у текущего объекта
        Animator targetAnimator = orbitCamera.target.GetComponent<Animator>();
        activateButton.SetActive(targetAnimator != null); // Показываем кнопку, только если Animator существует
    }
}