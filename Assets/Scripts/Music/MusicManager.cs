using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    public AudioSource musicSource; // �������� ������
    public Slider volumeSlider; // �������� ��� ���������� ����������
    public Button muteButton; // ������ ��� ���������� ������
    public float fadeDuration = 1.5f; // ������������ ���������

    private bool isMuted = false; // ���� ��� ������������ ��������� �����
    private float savedVolume; // ��� �������� ���������� ��������� �� ����������

    private void Awake()
    {
        // ��������� ������� ������ Singleton (���� ����� ��������� ���� ��������� �� �����)
        if (FindObjectsOfType<MusicManager>().Length > 1)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // �������� �� ������� ��������� ������
        if (musicSource == null)
        {
            Debug.LogError("MusicSource �� �������� � ����������.");
            return;
        }

        // �������� ���������� ��������� ��� ��������� ������������ ��������
        float savedVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        musicSource.volume = savedVolume;

        // ��������� ��������� ��������� ��������
        if (volumeSlider != null)
        {
            volumeSlider.value = savedVolume;
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }

        // ���������� ������ MuteUnmute �� ������
        if (muteButton != null)
        {
            muteButton.onClick.AddListener(MuteUnmute);
        }

        // ������������� �� ������� ����� ����� ��� �������� ���������
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    // ����� ��� ��������� ���������
    public void SetVolume(float volume)
    {
        if (!isMuted)
        {
            musicSource.volume = volume;
            PlayerPrefs.SetFloat("MusicVolume", volume); // ���������� ���������
            PlayerPrefs.Save();
        }
    }

    // ����� ��� ���������/���������� �����
    public void MuteUnmute()
    {
        if (isMuted)
        {
            // �������� ����
            musicSource.volume = savedVolume;
            PlayerPrefs.SetFloat("MusicVolume", savedVolume); // ���������� ���������
            isMuted = false;
        }
        else
        {
            // ��������� ����, �������� ������� ���������
            savedVolume = musicSource.volume;
            musicSource.volume = 0;
            isMuted = true;
        }

        PlayerPrefs.Save();
    }

    // ������� ��������� ������ ����� ������ �����
    private IEnumerator FadeOutMusic()
    {
        float startVolume = musicSource.volume;

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            musicSource.volume = Mathf.Lerp(startVolume, 0, t / fadeDuration);
            yield return null;
        }

        musicSource.volume = 0;
    }

    // �����, ������� ����� ���������� ��� �������� �����
    private void OnSceneUnloaded(Scene current)
    {
        // ������ �������� �������� ���������
        StartCoroutine(FadeOutMusic());
    }

    // �����, ������� ����� ���������� ��� �������� ����� �����
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // ��������������� ��������� ����� �������� ����� �����
        float savedVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        musicSource.volume = isMuted ? 0 : savedVolume;

        // ������������� �������� �������� �� ���������� ��������, ���� �������� ����������
        if (volumeSlider != null)
        {
            volumeSlider.value = savedVolume;
        }
    }

    private void OnDestroy()
    {
        // ������������ �� ������� ��� ����������� �������
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }
}
