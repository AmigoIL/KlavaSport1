using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Обязательно для работы с UI

public class PanelToggleOnEsc : MonoBehaviour
{
    public GameObject panel; // Панель, которую нужно открывать/закрывать
    public TypingTrainer typingTrainer; // Ссылка на скрипт тренажера печати
    public Button resumeButton; // Кнопка для закрытия панели и возобновления времени

    private void Start()
    {
        // Проверяем, что кнопка назначена, и добавляем к ней обработчик
        if (resumeButton != null)
        {
            resumeButton.onClick.AddListener(ResumeGame); // Добавляем обработчик нажатия
        }
        else
        {
            Debug.LogError("Кнопка продолжения не назначена в инспекторе.");
        }

        // Проверка на наличие тренажера печати
        if (typingTrainer == null)
        {
            Debug.LogError("Тренажер печати не назначен в инспекторе.");
        }
    }

    private void Update()
    {
        // Проверка на нажатие клавиши Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePanel();
        }
    }

    // Метод для переключения видимости панели и установки паузы
    private void TogglePanel()
    {
        if (panel != null)
        {
            bool isActive = !panel.activeSelf; // Переключаем видимость панели
            panel.SetActive(isActive);

            // Проверка на наличие тренажера и установка паузы
            if (typingTrainer != null)
            {
                typingTrainer.SetPause(isActive); // Установка паузы
            }
            else
            {
                Debug.LogWarning("Скрипт тренажера печати не назначен.");
            }
        }
        else
        {
            Debug.LogError("Панель не назначена в инспекторе.");
        }
    }

    // Метод для закрытия панели и продолжения игры
    public void ResumeGame()
    {
        if (panel != null)
        {
            panel.SetActive(false); // Скрыть панель

            // Возобновляем игру, если скрипт назначен
            if (typingTrainer != null)
            {
                typingTrainer.SetPause(false); // Возобновить время
            }
            else
            {
                Debug.LogWarning("Скрипт тренажера печати не назначен.");
            }
        }
        else
        {
            Debug.LogError("Панель не назначена в инспекторе.");
        }
    }
}
