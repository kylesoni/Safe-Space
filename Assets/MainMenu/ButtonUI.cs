using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonUI : MonoBehaviour
{
    public void HardModeButton()
    {
        Difficulty.difficulty = 1;
        SceneManager.LoadScene("Final");
    }

    public void EasyModeButton()
    {
        Difficulty.difficulty = 0;
        SceneManager.LoadScene("Final");
    }

    public void Menu()
    {
        Destroy(AudioManager.instance.gameObject);
        SceneManager.LoadScene("MainMenu");
    }

    public void ExitGame()
    {
        // UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
}
