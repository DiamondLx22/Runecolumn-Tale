using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUI : MonoBehaviour
{
    public void QuitGame()
    {
        Application.Quit();
        Debug.Break();
    }
}
