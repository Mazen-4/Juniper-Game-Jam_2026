using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public void Resume()
    {
        FindObjectOfType<PauseScript>().ResumeGame();
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Chamber1");
    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }
}