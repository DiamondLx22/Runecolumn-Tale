using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem; // Importiert das neue Input System

public class Pausemenu : MonoBehaviour
{
    public GameObject pausemenu;
    public static bool isPaused;

    // Reference to the Input Action Asset
    private InputAction pauseAction;

    void Start()
    {
        pausemenu.SetActive(false);

        // Initialisieren Sie die Input Actions
        var inputActionAsset = new PauseMenuInputActions();
        pauseAction = inputActionAsset.UI.Pause;
        pauseAction.Enable();

        // Fügen Sie den Callback für die Pause-Funktion hinzu
        pauseAction.performed += OnPause;
    }

    private void OnDestroy()
    {
        // Entfernen Sie den Callback, wenn das Objekt zerstört wird
        pauseAction.performed -= OnPause;
    }

    private void OnPause(InputAction.CallbackContext context)
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        pausemenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        pausemenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
