using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections; // ������ ��� ������������ ���� ��� ������������� IEnumerator

public class MusicManager : MonoBehaviour
{
    public AudioSource musicSource; // �������� ������
    public Slider volumeSlider; // �������� ��� ���������� ����������
    public Button muteButton; // ������ ��� ���������� ������
    public float fadeDuration = 1.5f; // ������������ ��������� ��� ����� �����

    private bool isMuted = false; // ���� ��� ������������ ��������� �����
    private float savedVolume; // ��� �������� ���������� ��������� �� ����������

    private void Start()
    {
        // �������� �� ������� ��������� ������
        if (musicSource == null)
        {
            Debug.LogError("MusicSource �� �������� � ����������.");
            return;
        }

        // ��������� ��������� ��������� ��������
        if (volumeSlider != null)
        {
            volumeSlider.value = musicSource.volume;
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }

        // ���������� ������ MuteUnmute �� ������
        if (muteButton != null)
        {
            muteButton.onClick.AddListener(MuteUnmute);
        }

        // ������������� �� ������� ����� �����
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // ����� ��� ��������� ���������
    public void SetVolume(float volume)
    {
        if (!isMuted)
        {
            musicSource.volume = volume;
        }
    }

    // ����� ��� ���������/���������� �����
    public void MuteUnmute()
    {
        if (isMuted)
        {
            // �������� ����
            musicSource.volume = savedVolume;
            isMuted = false;
        }
        else
        {
            // ��������� ����, �������� ������� ���������
            savedVolume = musicSource.volume;
            musicSource.volume = 0;
            isMuted = true;
        }
    }

    // ����� ��� ������������ ��������� ������
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

    // ������� ����� �����
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(FadeOutMusic());
    }

    private void OnDestroy()
    {
        // ������������ �� ������� ��� ����������� �������
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
