using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelController : MonoBehaviour
{
    public GameObject panel; // ������ �� ������ � ����������
    public Button toggleButton; // ������ �� ������ � ����������

    private void Start()
    {
        // ������������� �� ������� ������� ������
        toggleButton.onClick.AddListener(TogglePanel);
    }

    private void TogglePanel()
    {
        // ��������� ������, ���� ��� �������
        if (panel.activeSelf)
        {
            panel.SetActive(false);
        }
    }
}
