using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Story Item", menuName = "Items/Key Item")]
public class StoryItem : Item
{
    [Header("Optional Unlockable Object")]
    public GameObject unlockableObject; 

    public override void Use(PartyMember target)
    {
        Debug.Log($"Story item {itemName} was used. It triggered an event.");

        TriggerEvent();

        if (unlockableObject != null)
        {
            unlockableObject.SetActive(true);
            Debug.Log($"{itemName} unlocked {unlockableObject.name}!");
        }
    }

    private void TriggerEvent()
    {
        Debug.Log("Story event triggered.");
    }
}