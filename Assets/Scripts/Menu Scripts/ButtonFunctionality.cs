using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctionality : MonoBehaviour
{
    public void MainMenuClick() //Changes current scene to the NewMainMenu,  which replaces the placeholder main menu that was present.
    {
        SceneManager.LoadScene("NewMainMenu");
    }
}
