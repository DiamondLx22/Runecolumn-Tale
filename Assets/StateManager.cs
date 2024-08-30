using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StateManager : MonoBehaviour
{

    private GameController gameController;
    public StateInfo[] stateInfos;
    //  public List<StateInfo> stateInfos;
    [SerializeField] TextMeshProUGUI text_header, text_description;
    [SerializeField] private GameObject stateContainer;
    [SerializeField] private Image image;
    [SerializeField] private Button button;

    private void Awake()
    {
        gameController = FindObjectOfType<GameController>();
    }

    private void OnEnable()
    {
        GameState.StateAdded += NewStateCollected;
    }


    private void OnDisable()
    {
        GameState.StateAdded -= NewStateCollected;
    }

    void NewStateCollected(string id, int amount)
    {
        foreach (StateInfo stateInfo in stateInfos)
        {
            if (stateInfo.id == id)
            {
                text_header.SetText(stateInfo.itemName);
                text_description.SetText(stateInfo.description);
                image.sprite = stateInfo.icon;
            }
        }

        //print($"new Item collected with the id: {id} with the amount of");
        StartCoroutine(DelayOpenPanel());


    }

    IEnumerator DelayOpenPanel()
    {
        yield return null;
        stateContainer.SetActive(true);
        Selectable newSelection;
        newSelection = button;

        yield return null; // Wait for next Update() / next frame

        newSelection.Select();
        //gameController.StartStatePopUp();
    }

    public void CloseStatePopUp()
    {
        stateContainer.SetActive(false);
        //EventSystem.current.SetSelectedGameObject(null);
        //gameController.EndStatePopUpMode();
    }

    public StateInfo GetStateById(string id)
    {
        foreach (StateInfo stateInfo in stateInfos)
        {
            if (stateInfo.id == id)
            {
                return stateInfo;
            }
        }

        return null;
    }
}
