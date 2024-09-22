using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public int buttonIndex; // Индекс кнопки (1, 2 или 3)

    private void Start()
    {
        // Проверка наличия ButtonSoundManager
        if (ButtonSoundManager.Instance == null)
        {
            Debug.LogError("ButtonSoundManager не найден в сцене!");
            return;
        }

        // Добавление слушателя нажатия на кнопку
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OnButtonClick);
        }
        else
        {
            Debug.LogError("Компонент Button не найден на объекте!");
        }
    }

    private void OnButtonClick()
    {
        // Воспроизведение звука при нажатии
        ButtonSoundManager.Instance.PlayButtonSound(buttonIndex);
    }
}

