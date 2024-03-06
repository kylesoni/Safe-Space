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
}
