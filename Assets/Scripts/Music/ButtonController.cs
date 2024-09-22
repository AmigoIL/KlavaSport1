using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public int buttonIndex; // ������ ������ (1, 2 ��� 3)

    private void Start()
    {
        // �������� ������� ButtonSoundManager
        if (ButtonSoundManager.Instance == null)
        {
            Debug.LogError("ButtonSoundManager �� ������ � �����!");
            return;
        }

        // ���������� ��������� ������� �� ������
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OnButtonClick);
        }
        else
        {
            Debug.LogError("��������� Button �� ������ �� �������!");
        }
    }

    private void OnButtonClick()
    {
        // ��������������� ����� ��� �������
        ButtonSoundManager.Instance.PlayButtonSound(buttonIndex);
    }
}

