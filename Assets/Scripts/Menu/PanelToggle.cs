using UnityEngine;
using UnityEngine.UI;

public class PanelToggle : MonoBehaviour
{
    // Панель, которую нужно выключать
    public GameObject panel;

    // Кнопка, по нажатию на которую будет происходить переключение панели
    public Button toggleButton;

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

        // Назначаем метод TogglePanel() для события нажатия кнопки
        toggleButton.onClick.AddListener(TogglePanel);
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
        // Переключение видимости панели
        panel.SetActive(!panel.activeSelf);
    }
}
