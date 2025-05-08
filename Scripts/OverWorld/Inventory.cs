using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    public List<Item> items = new List<Item>(); 
    public int maxCapacity = 20; 

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject); 
        }
    }

    public bool AddItem(Item item)
    {
        if (items.Count < maxCapacity)
        {
            items.Add(item);
            Debug.Log($"{item.itemName} added to inventory.");
            return true;
        }
        else
        {
            Debug.Log("Inventory is full!");
            return false;
        }
    }

    public void RemoveItem(Item item)
    {
        if (items.Contains(item))
        {
            items.Remove(item);
            Debug.Log($"{item.itemName} removed from inventory.");
        }
    }

    public void UseItem(Item item, PartyMember target)
    {
        if (items.Contains(item))
        {
            item.Use(target);
            RemoveItem(item);
        }
        else
        {
            Debug.Log($"Item {item.itemName} is not in the inventory.");
        }
    }

    public void DisplayInventory()
    {
        Debug.Log("Inventory Contents:");
        foreach (var item in items)
        {
            Debug.Log($"- {item.itemName}: {item.description}");
        }
    }
}