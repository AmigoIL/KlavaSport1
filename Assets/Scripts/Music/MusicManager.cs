using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections; // Добавь это пространство имен для использования IEnumerator

public class MusicManager : MonoBehaviour
{
    public AudioSource musicSource; // Источник музыки
    public Slider volumeSlider; // Ползунок для управления громкостью
    public Button muteButton; // Кнопка для выключения музыки
    public float fadeDuration = 1.5f; // Длительность затухания при смене сцены

    private bool isMuted = false; // Флаг для отслеживания состояния звука
    private float savedVolume; // Для хранения предыдущей громкости до выключения

    private void Start()
    {
        // Проверка на наличие источника музыки
        if (musicSource == null)
        {
            Debug.LogError("MusicSource не назначен в инспекторе.");
            return;
        }

        // Установка начальной громкости ползунка
        if (volumeSlider != null)
        {
            volumeSlider.value = musicSource.volume;
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }

        // Назначение метода MuteUnmute на кнопку
        if (muteButton != null)
        {
            muteButton.onClick.AddListener(MuteUnmute);
        }

        // Подписываемся на событие смены сцены
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Метод для установки громкости
    public void SetVolume(float volume)
    {
        if (!isMuted)
        {
            musicSource.volume = volume;
        }
    }

    // Метод для включения/выключения звука
    public void MuteUnmute()
    {
        if (isMuted)
        {
            // Включить звук
            musicSource.volume = savedVolume;
            isMuted = false;
        }
        else
        {
            // Выключить звук, сохраняя текущую громкость
            savedVolume = musicSource.volume;
            musicSource.volume = 0;
            isMuted = true;
        }
    }

    // Метод для постепенного затухания музыки
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

    // Событие смены сцены
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(FadeOutMusic());
    }

    private void OnDestroy()
    {
        // Отписываемся от события при уничтожении объекта
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
