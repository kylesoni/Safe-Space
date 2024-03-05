using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Spaceship : MonoBehaviour
{
    public GameObject SpaceshipUI;
    public Button continueButton;
    public Button exitButton;

    void Start()
    {
        SpaceshipUI.SetActive(false);
    }

    public void ShowSpaceshipUI()
    {       
        Time.timeScale = 0f;
        SpaceshipUI.SetActive(true);        
    }

    public void ContinueGame()
    {       
        Time.timeScale = 1f;
        SpaceshipUI.SetActive(false);
    }

    public void ExitGame()
    {     
        // Exit the game
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Stops the play mode in the Unity Editor
        #else
                    Application.Quit(); // Quits the application in a standalone build
        #endif
    }
}
