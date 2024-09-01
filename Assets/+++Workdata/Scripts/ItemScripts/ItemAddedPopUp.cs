using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemAddedPopUp : MonoBehaviour
{
    public static event Action<ItemAddedPopUp> Confirmed;
    [SerializeField] private Button continueButton;

    private void Awake()
    {
        continueButton.onClick.AddListener(() =>
        {
            Confirmed?.Invoke(this);
        });
    }
}
