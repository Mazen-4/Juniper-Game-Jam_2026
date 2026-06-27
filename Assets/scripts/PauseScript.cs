using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScript : MonoBehaviour
{
    [SerializeField] private string pauseMenuSceneName = "PauseScreen";
    private bool isPaused = false;
    private PlayerMovement player;

    private void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    private void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        if (player != null) player.enabled = false;
        SceneManager.LoadScene(pauseMenuSceneName, LoadSceneMode.Additive);
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        if (player != null) player.enabled = true;
        SceneManager.UnloadSceneAsync(pauseMenuSceneName);
    }
}