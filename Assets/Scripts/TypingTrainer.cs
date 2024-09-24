using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using Random = UnityEngine.Random;

public class TypingTrainer : MonoBehaviour
{
    public TextMeshProUGUI wordText; // Object to display the word
    public RectTransform wordPanel; // Panel for automatic resizing
    public string[] wordsToType; // Array of words for the sequence
    public float delayBeforeNextWord = 1.5f; // Delay before the next word
    public float timeToTypeWord = 10f; // Time to type one word
    private int currentWordIndex = 0; // Index of the current word
    private string currentWord; // Current word to type
    private string currentInput = ""; // Current player input
    private bool isWrong = false; // Input error
    public TMP_Text Timer;
    public float timeStart = 10f; // Time to start the countdown
    private float wordTimer; // Timer for typing the word
    public GameObject word;
    public GameObject timeup;
    public GameObject victoryMessage; // UI object for the victory message

    void Start()
    {
        Timer.text = timeStart.ToString();
        SetNextWord(); // Set the first word
        victoryMessage.SetActive(false); // Ensure victory message is hidden at start
    }

    void Update()
    {
        if (timeStart > 0)
        {
            timeStart -= Time.deltaTime;
            Timer.text = Math.Round(timeStart).ToString();
        }
        else
        {
            word.SetActive(false);
            timeup.SetActive(true);
        }

        if (!isWrong)
        {
            wordTimer -= Time.deltaTime;
            if (wordTimer <= 0)
            {
                ResetWord(); // Time to type the word has expired
            }
            else
            {
                CheckInput();
            }
        }
    }

    // Set the next word
    void SetNextWord()
    {
        if (currentWordIndex < wordsToType.Length)
        {
            currentWord = wordsToType[currentWordIndex];
            currentWordIndex++;
            DisplayWord();
            wordTimer = timeToTypeWord; // Reset timer for the new word
        }
        else
        {
            DisplayVictoryMessage(); // Display victory message if all words are typed
        }
    }

    // Display the word and adjust the panel
    void DisplayWord()
    {
        wordText.text = currentWord;
        wordText.color = Color.white; // Default color
        currentInput = ""; // Reset current input
        AdjustPanelSize(); // Automatically resize the panel
    }

    // Check the input
    void CheckInput()
    {
        foreach (char c in Input.inputString)
        {
            if (currentInput.Length < currentWord.Length && c == currentWord[currentInput.Length])
            {
                currentInput += c;
                UpdateWordDisplay();
                AnimateLetter(currentInput.Length - 1); // Increase only the current letter
            }
            else
            {
                StartCoroutine(ShakeWord()); // Error — shake the word
                ResetWord();
                return;
            }
        }
    }

    // Update the word display
    void UpdateWordDisplay()
    {
        string newText = "";
        for (int i = 0; i < currentWord.Length; i++)
        {
            if (i < currentInput.Length)
            {
                newText += $"<color=#80C080>{currentWord[i]}</color>"; // Less bright green color
            }
            else
            {
                newText += currentWord[i];
            }
        }
        wordText.text = newText;

        // If the word is fully typed
        if (currentInput == currentWord)
        {
            StartCoroutine(ShakeWord()); // Successful completion — shake the word
            StartCoroutine(RemoveWordAfterDelay());
        }
    }

    // Animate the increase of the current letter
    void AnimateLetter(int index)
    {
        StartCoroutine(ShakeLetter(index));
    }

    // Coroutine to enlarge and shrink a specific letter
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

    // Coroutine to shake the entire word
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

    // Reset the word
    void ResetWord()
    {
        currentInput = "";
        isWrong = true;
        wordText.color = Color.red;
        StartCoroutine(ShakeWord()); // Shake the word on reset
        Invoke("ResetColor", 1f); // Reset color after 1 second
    }

    // Restore the original state of the word
    void ResetColor()
    {
        isWrong = false;
        DisplayWord();
    }

    // Coroutine to remove the word after typing
    IEnumerator RemoveWordAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeNextWord); // Delay before disappearing
        wordText.text = "";
        SetNextWord(); // Move to the next word
    }

    // Display the victory message and stop the game
    void DisplayVictoryMessage()
    {
        victoryMessage.SetActive(true); // Show victory message
        SetPause(true); // Pause the game
        word.SetActive(false); // Hide the word panel
    }

    // Automatic panel size adjustment based on word length
    void AdjustPanelSize()
    {
        Vector2 newSize = new Vector2(wordText.preferredWidth + 20, wordText.preferredHeight + 20);
        wordPanel.sizeDelta = newSize;
    }

    public void SetPause(bool isPaused)
    {
        if (isPaused)
        {
            Time.timeScale = 0f; // Pauses the game
        }
        else
        {
            Time.timeScale = 1f; // Resumes the game
        }
    }
}
