using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StateInfo
{
    public string id;

    public string itemName;

    public string description;

    public Sprite icon;

    public int amount;

    public StateCategorys category;
}

[System.Serializable]
public enum StateCategorys
{
    defaul,
    armor,
    staff,
    swords
}