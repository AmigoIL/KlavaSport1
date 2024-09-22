using UnityEngine;
using UnityEngine.UI;

public class MusicController : MonoBehaviour
{
    // ������ �� �����-��������
    public AudioSource musicSource;

    // ������ �� UI Slider
    public Slider volumeSlider;

    void Start()
    {
        // ������������� ��������� ������ � ����������� �� ���������� �������� ��������
        musicSource.volume = volumeSlider.value;

        // ������������� �� ������� ��������� �������� ��������
        volumeSlider.onValueChanged.AddListener(ChangeVolume);
    }

    // ����� ��� ��������� ���������
    public void ChangeVolume(float value)
    {
        musicSource.volume = value;
    }
}
