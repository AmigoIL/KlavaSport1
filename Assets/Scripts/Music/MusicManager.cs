using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    public AudioSource musicSource; // Источник музыки
    public Slider volumeSlider; // Ползунок для управления громкостью
    public Button muteButton; // Кнопка для выключения музыки
    public float fadeDuration = 1.5f; // Длительность затухания

    private bool isMuted = false; // Флаг для отслеживания состояния звука
    private float savedVolume; // Для хранения предыдущей громкости до выключения

    private void Awake()
    {
        // Назначаем текущий объект Singleton (если нужно сохранить один экземпляр на сцене)
        if (FindObjectsOfType<MusicManager>().Length > 1)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Проверка на наличие источника музыки
        if (musicSource == null)
        {
            Debug.LogError("MusicSource не назначен в инспекторе.");
            return;
        }

        // Загрузка сохранённой громкости или установка стандартного значения
        float savedVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        musicSource.volume = savedVolume;

        // Установка начальной громкости ползунка
        if (volumeSlider != null)
        {
            volumeSlider.value = savedVolume;
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }

        // Назначение метода MuteUnmute на кнопку
        if (muteButton != null)
        {
            muteButton.onClick.AddListener(MuteUnmute);
        }

        // Подписываемся на событие смены сцены для плавного затухания
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    // Метод для установки громкости
    public void SetVolume(float volume)
    {
        if (!isMuted)
        {
            musicSource.volume = volume;
            PlayerPrefs.SetFloat("MusicVolume", volume); // Сохранение громкости
            PlayerPrefs.Save();
        }
    }

    // Метод для включения/выключения звука
    public void MuteUnmute()
    {
        if (isMuted)
        {
            // Включить звук
            musicSource.volume = savedVolume;
            PlayerPrefs.SetFloat("MusicVolume", savedVolume); // Сохранение громкости
            isMuted = false;
        }
        else
        {
            // Выключить звук, сохраняя текущую громкость
            savedVolume = musicSource.volume;
            musicSource.volume = 0;
            isMuted = true;
        }

        PlayerPrefs.Save();
    }

    // Плавное затухание музыки перед сменой сцены
    private IEnumerator FadeOutMusic()
    {
        float startVolume = musicSource.volume;

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            musicSource.volume = Mathf.Lerp(startVolume, 0, t / fadeDuration);
            yield return null;
        }

        musicSource.volume = 0;
    }

    // Метод, который будет вызываться при выгрузке сцены
    private void OnSceneUnloaded(Scene current)
    {
        // Запуск корутины плавного затухания
        StartCoroutine(FadeOutMusic());
    }

    // Метод, который будет вызываться при загрузке новой сцены
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Восстанавливаем громкость после загрузки новой сцены
        float savedVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        musicSource.volume = isMuted ? 0 : savedVolume;

        // Устанавливаем значение ползунка на сохранённое значение, если ползунок существует
        if (volumeSlider != null)
        {
            volumeSlider.value = savedVolume;
        }
    }

    private void OnDestroy()
    {
        // Отписываемся от событий при уничтожении объекта
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }
}
