using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssentialLoader : MonoBehaviour
{
    public GameObject essentialContainer;
    public void Awake()
    {
        DontDestroyOnLoad(essentialContainer);
    }
}
