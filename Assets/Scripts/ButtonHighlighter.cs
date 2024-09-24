using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHighlighter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public RectTransform highlightBar; // Полоска для выделения
    public float highlightWidth = 100f; // Ширина полоски, которая появится
    public float animationDuration = 0.1f; // Длительность анимации

    private Vector2 originalSize; // Начальный размер полоски

    private void Start()
    {
        // Проверяем, что highlightBar назначен
        if (highlightBar != null)
        {
            // Сохраняем начальный размер полоски
            originalSize = highlightBar.sizeDelta;
            highlightBar.sizeDelta = new Vector2(0, originalSize.y); // Устанавливаем ширину 0
        }
        else
        {
            Debug.LogError("HighlightBar не назначен в инспекторе.");
        }
    }

    // Когда курсор входит в область кнопки
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (highlightBar != null)
        {
            StopAllCoroutines();
            StartCoroutine(AnimateHighlightBar(highlightWidth));
        }
    }

    // Когда курсор выходит из области кнопки
    public void OnPointerExit(PointerEventData eventData)
    {
        if (highlightBar != null)
        {
            StopAllCoroutines();
            StartCoroutine(AnimateHighlightBar(0));
        }
    }

    // Анимация изменения размера полоски
    private IEnumerator AnimateHighlightBar(float targetWidth)
    {
        float elapsedTime = 0f;
        Vector2 startSize = highlightBar.sizeDelta;
        Vector2 targetSize = new Vector2(targetWidth, originalSize.y);

        // Используем Time.unscaledDeltaTime для анимации во время паузы
        while (elapsedTime < animationDuration)
        {
            highlightBar.sizeDelta = Vector2.Lerp(startSize, targetSize, elapsedTime / animationDuration);
            elapsedTime += Time.unscaledDeltaTime; // Используем unscaledDeltaTime, чтобы анимация не зависела от Time.timeScale
            yield return null;
        }

        highlightBar.sizeDelta = targetSize; // Устанавливаем окончательный размер
    }
}
