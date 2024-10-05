using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;
using System;
using Random = UnityEngine.Random;

public class TypingTrainer : MonoBehaviour
{
    public TextMeshProUGUI wordText; // Object to display the word
    public RectTransform wordPanel; // Panel for automatic resizing
    public float delayBeforeNextWord = 1.5f; // Delay before the next word
    public float timeToTypeWord = 10f; // Time to type one word
    private List<string> wordsToType; // List of words for the sequence
    private int currentWordIndex = 0; // Index of the current word
    private string currentWord; // Current word to type
    private string currentInput = ""; // Current player input
    private bool isWrong = false; // Input error
    public TMP_Text Timer;
    public float initialTimeStart = 10f; // Initial time value
    private float timeStart; // Current time variable
    private float wordTimer; // Timer for typing the word
    public GameObject word;
    public GameObject timeup;
    public GameObject victoryMessage; // UI object for the victory message
    public Button restartButton; // Button for restarting the game


    void Start()
    {
        timeStart = initialTimeStart; // Set the starting time
        Timer.text = timeStart.ToString();
        LoadWordsFromFile(); // Load words from the external file
        SetNextWord(); // Set the first word
        victoryMessage.SetActive(false); // Ensure victory message is hidden at start
        restartButton.gameObject.SetActive(false); // Hide restart button at the start
        restartButton.onClick.AddListener(RestartGame); // Add listener to the button
    }

    void Update()
    {
        if (timeStart > 0)
        {
            timeStart -= Time.deltaTime; // Decrease current time
            Timer.text = Math.Round(timeStart).ToString(); // Update timer text
        }
        else
        {
            GameOver(); // Game over when the time runs out
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

    // Load words from an external file
    void LoadWordsFromFile()
    {
        wordsToType = new List<string>(); // Initialize the list

        string filePath = Path.Combine(Application.streamingAssetsPath, "words.txt"); // Set the file path

        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath); // Read all lines from the file
            wordsToType.AddRange(lines); // Add lines to the list
        }
        else
        {
            Debug.LogError("File not found: " + filePath);
        }
    }

    // Set the next word randomly
    void SetNextWord()
    {
        if (wordsToType.Count > 0)
        {
            currentWordIndex = Random.Range(0, wordsToType.Count); // Randomly select an index
            currentWord = wordsToType[currentWordIndex]; // Get the word from the list
            wordsToType.RemoveAt(currentWordIndex); // Remove it to avoid repetition
            DisplayWord();
            wordTimer = timeToTypeWord; // Reset timer for the new word
        }
        else
        {
            // If there are no words left, show victory message
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

        // Check if any words are left, if not, show victory
        if (wordsToType.Count <= 0)
        {
            DisplayVictoryMessage(); // Victory, all words typed
        }
        else
        {
            SetNextWord(); // Move to the next word
        }
    }

    // Display the victory message and stop the game
    void DisplayVictoryMessage()
    {
        victoryMessage.SetActive(true); // Show victory message
        SetPause(true); // Pause the game
        word.SetActive(false); // Hide the word panel
    }

    // Show game over state
    void GameOver()
    {
        word.SetActive(false); // Hide the word panel
        timeup.SetActive(true); // Show the time-up message
        restartButton.gameObject.SetActive(true); // Show the restart button
    }

    // Restart the game
    void RestartGame()
    {
        timeStart = initialTimeStart; // Reset current time to initial value
        isWrong = false;
        restartButton.gameObject.SetActive(false); // Hide the restart button
        timeup.SetActive(false); // Hide time-up message
        victoryMessage.SetActive(false); // Hide victory message
        word.SetActive(true); // Show the word panel
        LoadWordsFromFile(); // Reload words from file
        SetNextWord(); // Start the first word again
        SetPause(false); // Unpause the game
    }

    // Automatic panel size adjustment based on word length
    void AdjustPanelSize()
    {
        Vector2 newSize = new Vector2(wordText.preferredWidth + 20, wordText.preferredHeight + 20);
        wordPanel.sizeDelta = newSize;
    }

    public void SetPause(bool isPaused)
    {
        Time.timeScale = isPaused ? 0f : 1f; // Pauses or resumes the game
    }
}
