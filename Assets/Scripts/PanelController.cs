using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelController : MonoBehaviour
{
    public GameObject panel; // Ссылка на панель в инспекторе
    public Button toggleButton; // Ссылка на кнопку в инспекторе

    private void Start()
    {
        // Подписываемся на событие нажатия кнопки
        toggleButton.onClick.AddListener(TogglePanel);
    }

    private void TogglePanel()
    {
        // Выключаем панель, если она активна
        if (panel.activeSelf)
        {
            panel.SetActive(false);
        }
    }
}
