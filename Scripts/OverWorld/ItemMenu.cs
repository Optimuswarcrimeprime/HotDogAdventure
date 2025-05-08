using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemMenu : MonoBehaviour
{
    public GameObject itemButtonPrefab; 
    public Transform itemListParent; 
    public Inventory inventory;  
    public PartyManager partyManager;
    public GameObject partyMemberSelectionPanel;
    public Transform partyMemberListParent; 
    public GameObject partyMemberButtonPrefab; 
    public TextMeshProUGUI itemDescriptionText; 
    public Image itemIcon;

    private Item selectedItem;


    void OnEnable()
    {
        PopulateItemList(); 
        HidePartyMemberSelection(); 
        ClearItemDetails();
    }

    void PopulateItemList()
    {
        foreach (Transform child in itemListParent)
        {
            Destroy(child.gameObject);
        }

        foreach (Item item in inventory.items)
        {
            GameObject button = Instantiate(itemButtonPrefab, itemListParent);
            button.GetComponentInChildren<TextMeshProUGUI>().text = item.itemName;

            button.GetComponent<Button>().onClick.AddListener(() => SelectItem(item));

            if (!CanUseItem(item))
            {
                button.GetComponent<Button>().interactable = false; 
            }
        }
    }

    void SelectItem(Item item)
    {
        selectedItem = item; 
        ShowItemDetails(item); 
    }

    void ShowItemDetails(Item item)
    {
        itemDescriptionText.text = item.description;
        itemIcon.sprite = item.icon;
        itemIcon.gameObject.SetActive(true);
    }

    void ClearItemDetails()
    {
        itemDescriptionText.text = ""; 
        itemIcon.sprite = null; 
        itemIcon.gameObject.SetActive(false); 
    }

    public void UseSelectedItem()
    {
        if (selectedItem != null)
        {
            ShowPartyMemberSelection(); 
        }
    }

    void ShowPartyMemberSelection()
    {
        partyMemberSelectionPanel.SetActive(true); 

        foreach (Transform child in partyMemberListParent)
        {
            Destroy(child.gameObject); 
        }

        foreach (PartyMember member in partyManager.GetCurrentParty())
        {
            GameObject button = Instantiate(partyMemberButtonPrefab, partyMemberListParent); 
            button.GetComponentInChildren<TextMeshProUGUI>().text = member.MemberName; 

            button.GetComponent<Button>().onClick.AddListener(() => UseItemOnPartyMember(member));
        }
    }

    public void HidePartyMemberSelection()
    {
        partyMemberSelectionPanel.SetActive(false);
    }

    void UseItemOnPartyMember(PartyMember member)
    {
        inventory.UseItem(selectedItem, member);
        PopulateItemList(); 
        HidePartyMemberSelection();
        ClearItemDetails();
    }


    bool CanUseItem(Item item)
    {
        if (item is HealingItem healingItem)
        {
            foreach (PartyMember member in partyManager.GetCurrentParty())
            {
                if (member.CurrentHealth < member.MaxHealth)
                {
                    return true; 
                }
            }
            return false;  
        }
        return true; 
    }
}
