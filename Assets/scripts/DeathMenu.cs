using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    public void Retry()
    {
        SceneManager.LoadScene("Chamber1");  // your game scene name
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");  // your main menu scene name
    }
}