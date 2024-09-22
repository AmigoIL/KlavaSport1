using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Для использования SceneManager

public class PanelToggle : MonoBehaviour
{
    // Панель, которую нужно выключать
    public GameObject panel;

    // Кнопка, по нажатию на которую будет происходить переключение панели
    public Button toggleButton;

    // Источник звука для воспроизведения
    public AudioSource buttonAudioSource;

    // Звук, который будет воспроизводиться при переходе на сцену
    public AudioClip buttonSoundClip;

    private void Start()
    {
        // Проверка на случай, если панель или кнопка не заданы
        if (panel == null)
        {
            Debug.LogError("Панель не назначена в инспекторе.");
            return;
        }

        if (toggleButton == null)
        {
            Debug.LogError("Кнопка не назначена в инспекторе.");
            return;
        }

        if (buttonAudioSource == null)
        {
            Debug.LogError("Источник звука не назначен в инспекторе.");
            return;
        }

        // Назначаем метод TogglePanel() для события нажатия кнопки
        toggleButton.onClick.AddListener(TogglePanel);

        // Подписываемся на событие смены сцены
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Update()
    {
        // Проверка на нажатие клавиши Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Выключаем панель, если она активна
            if (panel.activeSelf)
            {
                panel.SetActive(false);
            }
        }
    }

    // Метод для включения/выключения панели по кнопке
    public void TogglePanel()
    {
        // Воспроизводим звук при нажатии кнопки
        PlayButtonSound();

        // Переключение видимости панели
        panel.SetActive(!panel.activeSelf);
    }

    // Метод для воспроизведения звука кнопки
    private void PlayButtonSound()
    {
        if (buttonAudioSource != null && buttonSoundClip != null)
        {
            buttonAudioSource.PlayOneShot(buttonSoundClip);
        }
        else
        {
            Debug.LogError("Источник звука или клип не назначены в инспекторе.");
        }
    }

    // Метод, который вызывается при загрузке новой сцены
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Воспроизводим звук при загрузке новой сцены
        PlayButtonSound();
    }

    private void OnDestroy()
    {
        // Отписываемся от события при уничтожении объекта
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
