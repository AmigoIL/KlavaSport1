using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // ��� ������������� SceneManager

public class PanelToggle : MonoBehaviour
{
    // ������, ������� ����� ���������
    public GameObject panel;

    // ������, �� ������� �� ������� ����� ����������� ������������ ������
    public Button toggleButton;

    // �������� ����� ��� ���������������
    public AudioSource buttonAudioSource;

    // ����, ������� ����� ���������������� ��� �������� �� �����
    public AudioClip buttonSoundClip;

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

        if (buttonAudioSource == null)
        {
            Debug.LogError("�������� ����� �� �������� � ����������.");
            return;
        }

        // ��������� ����� TogglePanel() ��� ������� ������� ������
        toggleButton.onClick.AddListener(TogglePanel);

        // ������������� �� ������� ����� �����
        SceneManager.sceneLoaded += OnSceneLoaded;
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
        // ������������� ���� ��� ������� ������
        PlayButtonSound();

        // ������������ ��������� ������
        panel.SetActive(!panel.activeSelf);
    }

    // ����� ��� ��������������� ����� ������
    private void PlayButtonSound()
    {
        if (buttonAudioSource != null && buttonSoundClip != null)
        {
            buttonAudioSource.PlayOneShot(buttonSoundClip);
        }
        else
        {
            Debug.LogError("�������� ����� ��� ���� �� ��������� � ����������.");
        }
    }

    // �����, ������� ���������� ��� �������� ����� �����
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // ������������� ���� ��� �������� ����� �����
        PlayButtonSound();
    }

    private void OnDestroy()
    {
        // ������������ �� ������� ��� ����������� �������
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
