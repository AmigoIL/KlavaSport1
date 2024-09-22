using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // ����������� ��� ������ � UI

public class PanelToggleOnEsc : MonoBehaviour
{
    public GameObject panel; // ������, ������� ����� ���������/���������
    public TypingTrainer typingTrainer; // ������ �� ������ ��������� ������
    public Button resumeButton; // ������ ��� �������� ������ � ������������� �������

    private void Start()
    {
        // ���������, ��� ������ ���������, � ��������� � ��� ����������
        if (resumeButton != null)
        {
            resumeButton.onClick.AddListener(ResumeGame); // ��������� ���������� �������
        }
        else
        {
            Debug.LogError("������ ����������� �� ��������� � ����������.");
        }

        // �������� �� ������� ��������� ������
        if (typingTrainer == null)
        {
            Debug.LogError("�������� ������ �� �������� � ����������.");
        }
    }

    private void Update()
    {
        // �������� �� ������� ������� Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePanel();
        }
    }

    // ����� ��� ������������ ��������� ������ � ��������� �����
    private void TogglePanel()
    {
        if (panel != null)
        {
            bool isActive = !panel.activeSelf; // ����������� ��������� ������
            panel.SetActive(isActive);

            // �������� �� ������� ��������� � ��������� �����
            if (typingTrainer != null)
            {
                typingTrainer.SetPause(isActive); // ��������� �����
            }
            else
            {
                Debug.LogWarning("������ ��������� ������ �� ��������.");
            }
        }
        else
        {
            Debug.LogError("������ �� ��������� � ����������.");
        }
    }

    // ����� ��� �������� ������ � ����������� ����
    public void ResumeGame()
    {
        if (panel != null)
        {
            panel.SetActive(false); // ������ ������

            // ������������ ����, ���� ������ ��������
            if (typingTrainer != null)
            {
                typingTrainer.SetPause(false); // ����������� �����
            }
            else
            {
                Debug.LogWarning("������ ��������� ������ �� ��������.");
            }
        }
        else
        {
            Debug.LogError("������ �� ��������� � ����������.");
        }
    }
}
