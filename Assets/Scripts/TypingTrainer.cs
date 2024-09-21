using System.Collections;
using UnityEngine;
using TMPro;

public class TypingTrainer : MonoBehaviour
{
    public TextMeshProUGUI wordText; // ������ ��� ����������� �����
    public RectTransform wordPanel; // ������ ��� ��������������� ����������
    public string[] wordsToType; // ������ ���� ��� ������������������
    public float delayBeforeNextWord = 1.5f; // �������� ����� ��������� ������
    public float timeToTypeWord = 10f; // ����� �� ���� ������ �����
    private int currentWordIndex = 0; // ������ �������� �����
    private string currentWord; // ������� ����� ��� �����
    private string currentInput = ""; // ������� ���� ������
    private bool isWrong = false; // ������ �����
    private float wordTimer; // ������ �� ���� �����

    void Start()
    {
        SetNextWord(); // ������ ������ �����
    }

    void Update()
    {
        if (!isWrong)
        {
            wordTimer -= Time.deltaTime;
            if (wordTimer <= 0)
            {
                ResetWord(); // ����� �� ���� ����� �������
            }
            else
            {
                CheckInput();
            }
        }
    }

    // ��������� ���������� �����
    void SetNextWord()
    {
        if (currentWordIndex < wordsToType.Length)
        {
            currentWord = wordsToType[currentWordIndex];
            currentWordIndex++;
            DisplayWord();
            wordTimer = timeToTypeWord; // ����� ������� ��� ������ �����
        }
        else
        {
            Debug.Log("��� ����� �������!");
            wordText.text = "";
        }
    }

    // ����������� ����� � ��������� ������
    void DisplayWord()
    {
        wordText.text = currentWord;
        wordText.color = Color.white; // ����� ���� �� ���������
        currentInput = ""; // ���������� ������� ����
        AdjustPanelSize(); // �������������� ��������� ������� ������
    }

    // �������� �����
    void CheckInput()
    {
        foreach (char c in Input.inputString)
        {
            if (currentInput.Length < currentWord.Length && c == currentWord[currentInput.Length])
            {
                currentInput += c;
                UpdateWordDisplay();
                AnimateLetter(currentInput.Length - 1); // ����������� ������ ������� �����
            }
            else
            {
                StartCoroutine(ShakeWord()); // ������ � ������ �����
                ResetWord();
                return;
            }
        }
    }

    // ���������� ����������� �����
    void UpdateWordDisplay()
    {
        string newText = "";
        for (int i = 0; i < currentWord.Length; i++)
        {
            if (i < currentInput.Length)
            {
                newText += $"<color=#80C080>{currentWord[i]}</color>"; // ����� ����� ������� ����
            }
            else
            {
                newText += currentWord[i];
            }
        }
        wordText.text = newText;

        // ���� ����� ��������� �������
        if (currentInput == currentWord)
        {
            StartCoroutine(ShakeWord()); // �������� ���������� � ������ �����
            StartCoroutine(RemoveWordAfterDelay());
        }
    }

    // �������� ���������� ������ ������� �����
    void AnimateLetter(int index)
    {
        StartCoroutine(ShakeLetter(index));
    }

    // �������� ��� ���������� � ���������� ���������� �����
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

    // �������� ��� ������ ����� �����
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

    // ����� �����
    void ResetWord()
    {
        currentInput = "";
        isWrong = true;
        wordText.color = Color.red;
        StartCoroutine(ShakeWord()); // ������ ����� ��� ������
        Invoke("ResetColor", 1f); // ����� ����� ����� 1 �������
    }

    // �������������� ��������� ��������� �����
    void ResetColor()
    {
        isWrong = false;
        DisplayWord();
    }

    // �������� ��� �������� ����� ����� �����
    IEnumerator RemoveWordAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeNextWord); // �������� ����� �������������
        wordText.text = "";
        SetNextWord(); // ������� � ���������� �����
    }

    // �������������� ��������� ������� ������ � ����������� �� ����� �����
    void AdjustPanelSize()
    {
        Vector2 newSize = new Vector2(wordText.preferredWidth + 20, wordText.preferredHeight + 20);
        wordPanel.sizeDelta = newSize;
    }
}
