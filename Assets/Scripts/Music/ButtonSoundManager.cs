using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonSoundManager : MonoBehaviour
{
    public static ButtonSoundManager Instance; // Синглтон для управления звуками

    // Звуки для каждой кнопки
    public AudioClip buttonSound1;
    public AudioClip buttonSound2;
    public AudioClip buttonSound3;

    private AudioSource audioSource;

    private void Awake()
    {
        // Создание синглтона
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Добавляем компонент AudioSource, если его нет
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    // Метод для воспроизведения звука
    public void PlayButtonSound(int buttonIndex)
    {
        switch (buttonIndex)
        {
            case 1:
                audioSource.PlayOneShot(buttonSound1);
                break;
            case 2:
                audioSource.PlayOneShot(buttonSound2);
                break;
            case 3:
                audioSource.PlayOneShot(buttonSound3);
                break;
            default:
                Debug.LogError("Неверный индекс кнопки!");
                break;
        }
    }
}
