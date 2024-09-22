using UnityEngine;
using UnityEngine.UI;

public class MusicController : MonoBehaviour
{
    // Ссылка на аудио-источник
    public AudioSource musicSource;

    // Ссылка на UI Slider
    public Slider volumeSlider;

    void Start()
    {
        // Устанавливаем громкость музыки в зависимости от начального значения ползунка
        musicSource.volume = volumeSlider.value;

        // Подписываемся на событие изменения значения ползунка
        volumeSlider.onValueChanged.AddListener(ChangeVolume);
    }

    // Метод для изменения громкости
    public void ChangeVolume(float value)
    {
        musicSource.volume = value;
    }
}
