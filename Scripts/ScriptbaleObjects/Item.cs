using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Item : ScriptableObject
{
    public string itemName;
    public string description;
    public Sprite icon;

    public abstract void Use(PartyMember target);
}
