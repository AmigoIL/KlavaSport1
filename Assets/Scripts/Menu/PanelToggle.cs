using UnityEngine;
using UnityEngine.UI;

public class PanelToggle : MonoBehaviour
{
    // ������, ������� ����� ���������
    public GameObject panel;

    // ������, �� ������� �� ������� ����� ����������� ������������ ������
    public Button toggleButton;

    private void Start()
    {
        // �������� �� ������, ���� ������ ��� ������ �� ������
        if (panel == null)
        {
            Debug.LogError("������ �� ��������� � ����������.");
            return;
        }

        if (toggleButton == null)
        {
            Debug.LogError("������ �� ��������� � ����������.");
            return;
        }

        // ��������� ����� TogglePanel() ��� ������� ������� ������
        toggleButton.onClick.AddListener(TogglePanel);
    }

    private void Update()
    {
        // �������� �� ������� ������� Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // ��������� ������, ���� ��� �������
            if (panel.activeSelf)
            {
                panel.SetActive(false);
            }
        }
    }

    // ����� ��� ���������/���������� ������ �� ������
    public void TogglePanel()
    {
        // ������������ ��������� ������
        panel.SetActive(!panel.activeSelf);
    }
}
