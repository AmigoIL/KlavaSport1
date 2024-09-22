using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public void LoadMenuScene()
    {
        SceneManager.LoadScene("SampleScene"); 
    }
    public void LoadMenuScene2()
    {
        SceneManager.LoadScene("Menu");
    }

    public void QuitGame()
    {
        // �������� ����������
        Application.Quit();

        // ��� �������� � ��������� Unity
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}

