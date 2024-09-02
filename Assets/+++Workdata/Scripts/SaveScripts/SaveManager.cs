using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public void SaveGame()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        PlayerPrefs.SetString("SceneName", currentScene.name);
        
        Transform playerTransform = FindObjectOfType<PlayerMovement>().transform;
        PlayerPrefs.SetFloat("PlayerPositionX", playerTransform.position.x);
        PlayerPrefs.SetFloat("PlayerPositionY", playerTransform.position.y);
        
        PlayerPrefs.Save();
    }

    public void LoadGame()
    {
        Transform playerTransform = FindObjectOfType<PlayerMovement>().transform;
        float posX = PlayerPrefs.GetFloat(key:"PlayerPositionX");
        float posY = PlayerPrefs.GetFloat(key:"PlayerPositionY");

        playerTransform.position = new Vector3(posX, posY, z: 0);
    }
}
