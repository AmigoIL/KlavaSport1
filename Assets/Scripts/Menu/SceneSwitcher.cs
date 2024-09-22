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
        // Закрытие приложения
        Application.Quit();

        // Для проверки в редакторе Unity
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}

