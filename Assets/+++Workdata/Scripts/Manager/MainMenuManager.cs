using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
  private GameController gameController;

  private void OnEnable()
  {
    gameController = FindObjectOfType<GameController>();

    switch (gameController.gameMode)
    {
     case GameController.GameMode.PreMenu:
       gameController.gameMode = GameController.GameMode.MainMenu;
       break;
    }
  }

  public void Button_NewGame()
  {
    gameController.gameMode = GameController.GameMode.NewGame;
    SceneManager.LoadScene("Overworld");
  }

  public void Button_Continue()
  {
    gameController.gameMode = GameController.GameMode.LoadGame;
    SceneManager.LoadScene(PlayerPrefs.GetString("Scene"));
  }

  public void Button_Quit()
  {
    Application.Quit();
  }
  
}
