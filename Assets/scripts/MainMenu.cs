using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    //public void ContinueGame()
    //{
    //    // same as NewGame for now
    //    SceneManager.LoadScene("GameScene");
    //}
    public void NewGame()
    {
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
