using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoader : MonoBehaviour
{
  [SerializeField] private Transform[] spawnpoints;
  private void OnEnable()
  {
    GameController gameController = FindObjectOfType<GameController>();
    SpawnpointSaver spawnpointSaver = FindObjectOfType<SpawnpointSaver>();
    Transform playerTransform = FindObjectOfType<PlayerMovement>().transform;
    
    switch (gameController.gameMode)
    {
      case GameController.GameMode.LoadGame:
        FindObjectOfType<SaveManager>().LoadGame();
        break;
      
      case GameController.GameMode.NewGame:
        playerTransform.position = spawnpoints[0].position;
        break;
      
      case GameController.GameMode.GameMode:
        playerTransform.position = spawnpoints[spawnpointSaver.spawnpointId].position;
        break;
      
      case GameController.GameMode.DebugMode:
        Debug.LogWarning(message: "Debug Mode turned On");
        break;
      
      default:
        Debug.LogWarning("Game Mode not supported!");
        playerTransform.position = spawnpoints[0].position;
        break;
    }
  }

}
