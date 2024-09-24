using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHighlighter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public RectTransform highlightBar; // ������� ��� ���������
    public float highlightWidth = 100f; // ������ �������, ������� ��������
    public float animationDuration = 0.1f; // ������������ ��������

    private Vector2 originalSize; // ��������� ������ �������

    private void Start()
    {
        // ���������, ��� highlightBar ��������
        if (highlightBar != null)
        {
            // ��������� ��������� ������ �������
            originalSize = highlightBar.sizeDelta;
            highlightBar.sizeDelta = new Vector2(0, originalSize.y); // ������������� ������ 0
        }
        else
        {
            Debug.LogError("HighlightBar �� �������� � ����������.");
        }
    }

    // ����� ������ ������ � ������� ������
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (highlightBar != null)
        {
            StopAllCoroutines();
            StartCoroutine(AnimateHighlightBar(highlightWidth));
        }
    }

    // ����� ������ ������� �� ������� ������
    public void OnPointerExit(PointerEventData eventData)
    {
        if (highlightBar != null)
        {
            StopAllCoroutines();
            StartCoroutine(AnimateHighlightBar(0));
        }
    }

    // �������� ��������� ������� �������
    private IEnumerator AnimateHighlightBar(float targetWidth)
    {
        float elapsedTime = 0f;
        Vector2 startSize = highlightBar.sizeDelta;
        Vector2 targetSize = new Vector2(targetWidth, originalSize.y);

        // ���������� Time.unscaledDeltaTime ��� �������� �� ����� �����
        while (elapsedTime < animationDuration)
        {
            highlightBar.sizeDelta = Vector2.Lerp(startSize, targetSize, elapsedTime / animationDuration);
            elapsedTime += Time.unscaledDeltaTime; // ���������� unscaledDeltaTime, ����� �������� �� �������� �� Time.timeScale
            yield return null;
        }

        highlightBar.sizeDelta = targetSize; // ������������� ������������� ������
    }
}
