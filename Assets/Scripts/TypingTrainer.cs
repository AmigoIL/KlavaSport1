using System.Collections;
using UnityEngine;
using TMPro;

public class TypingTrainer : MonoBehaviour
{
    public TextMeshProUGUI wordText; // Объект для отображения слова
    public RectTransform wordPanel; // Панель для автоматического увеличения
    public string[] wordsToType; // Массив слов для последовательности
    public float delayBeforeNextWord = 1.5f; // Задержка перед следующим словом
    public float timeToTypeWord = 10f; // Время на ввод одного слова
    private int currentWordIndex = 0; // Индекс текущего слова
    private string currentWord; // Текущее слово для ввода
    private string currentInput = ""; // Текущий ввод игрока
    private bool isWrong = false; // Ошибка ввода
    private float wordTimer; // Таймер на ввод слова

    void Start()
    {
        SetNextWord(); // Задаем первое слово
    }

    void Update()
    {
        if (!isWrong)
        {
            wordTimer -= Time.deltaTime;
            if (wordTimer <= 0)
            {
                ResetWord(); // Время на ввод слова истекло
            }
            else
            {
                CheckInput();
            }
        }
    }

    // Установка следующего слова
    void SetNextWord()
    {
        if (currentWordIndex < wordsToType.Length)
        {
            currentWord = wordsToType[currentWordIndex];
            currentWordIndex++;
            DisplayWord();
            wordTimer = timeToTypeWord; // Сброс таймера для нового слова
        }
        else
        {
            Debug.Log("Все слова введены!");
            wordText.text = "";
        }
    }

    // Отображение слова и настройка панели
    void DisplayWord()
    {
        wordText.text = currentWord;
        wordText.color = Color.white; // Белый цвет по умолчанию
        currentInput = ""; // Сбрасываем текущий ввод
        AdjustPanelSize(); // Автоматическое изменение размера панели
    }

    // Проверка ввода
    void CheckInput()
    {
        foreach (char c in Input.inputString)
        {
            if (currentInput.Length < currentWord.Length && c == currentWord[currentInput.Length])
            {
                currentInput += c;
                UpdateWordDisplay();
                AnimateLetter(currentInput.Length - 1); // Увеличиваем только текущую букву
            }
            else
            {
                StartCoroutine(ShakeWord()); // Ошибка — трясем слово
                ResetWord();
                return;
            }
        }
    }

    // Обновление отображения слова
    void UpdateWordDisplay()
    {
        string newText = "";
        for (int i = 0; i < currentWord.Length; i++)
        {
            if (i < currentInput.Length)
            {
                newText += $"<color=#80C080>{currentWord[i]}</color>"; // Менее яркий зеленый цвет
            }
            else
            {
                newText += currentWord[i];
            }
        }
        wordText.text = newText;

        // Если слово полностью введено
        if (currentInput == currentWord)
        {
            StartCoroutine(ShakeWord()); // Успешное завершение — трясем слово
            StartCoroutine(RemoveWordAfterDelay());
        }
    }

    // Анимация увеличения только текущей буквы
    void AnimateLetter(int index)
    {
        StartCoroutine(ShakeLetter(index));
    }

    // Корутина для увеличения и уменьшения конкретной буквы
    IEnumerator ShakeLetter(int index)
    {
        string newText = "";
        for (int i = 0; i < currentWord.Length; i++)
        {
            if (i == index)
            {
                newText += $"<size=150%><color=#80C080>{currentWord[i]}</color></size>";
            }
            else if (i < currentInput.Length)
            {
                newText += $"<color=#80C080>{currentWord[i]}</color>";
            }
            else
            {
                newText += currentWord[i];
            }
        }
        wordText.text = newText;
        yield return new WaitForSeconds(0.1f);
        UpdateWordDisplay();
    }

    // Корутина для тряски всего слова
    IEnumerator ShakeWord()
    {
        Vector3 originalPosition = wordText.transform.localPosition;
        float shakeAmount = 5f;
        for (int i = 0; i < 10; i++)
        {
            wordText.transform.localPosition = originalPosition + (Vector3)Random.insideUnitCircle * shakeAmount;
            yield return new WaitForSeconds(0.05f);
        }
        wordText.transform.localPosition = originalPosition;
    }

    // Сброс слова
    void ResetWord()
    {
        currentInput = "";
        isWrong = true;
        wordText.color = Color.red;
        StartCoroutine(ShakeWord()); // Тряска слова при сбросе
        Invoke("ResetColor", 1f); // Сброс цвета через 1 секунду
    }

    // Восстановление исходного состояния слова
    void ResetColor()
    {
        isWrong = false;
        DisplayWord();
    }

    // Корутина для удаления слова после ввода
    IEnumerator RemoveWordAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeNextWord); // Задержка перед исчезновением
        wordText.text = "";
        SetNextWord(); // Переход к следующему слову
    }

    // Автоматическая настройка размера панели в зависимости от длины слова
    void AdjustPanelSize()
    {
        Vector2 newSize = new Vector2(wordText.preferredWidth + 20, wordText.preferredHeight + 20);
        wordPanel.sizeDelta = newSize;
    }
}
