using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonSoundManager : MonoBehaviour
{
    public static ButtonSoundManager Instance; // �������� ��� ���������� �������

    // ����� ��� ������ ������
    public AudioClip buttonSound1;
    public AudioClip buttonSound2;
    public AudioClip buttonSound3;

    private AudioSource audioSource;

    private void Awake()
    {
        // �������� ���������
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

        // ��������� ��������� AudioSource, ���� ��� ���
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    // ����� ��� ��������������� �����
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
                Debug.LogError("�������� ������ ������!");
                break;
        }
    }
}
