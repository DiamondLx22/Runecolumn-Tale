using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UiManager : MonoBehaviour
{
    public bool option_menu = false;

    public GameObject option_screen;
    public GameObject mainMenu;
    private void OnEnable()
    {

    }

    public void StartGame()
    {
        SceneManager.LoadScene("MainWorld");
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void CallOptionMenu()
    {   
        option_menu = !option_menu;
        option_screen.SetActive(option_menu);
        mainMenu.SetActive(!option_menu);

    }
}

