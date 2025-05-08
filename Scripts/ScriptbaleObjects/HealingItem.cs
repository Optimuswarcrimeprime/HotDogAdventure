using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Healing Item", menuName = "Items/Healing Item")]
public class HealingItem : Item
{

    public int healAmount; 
   
    public override void Use(PartyMember target)
    {
        if (target.CurrentHealth < target.MaxHealth)
        {
            target.CurrentHealth += healAmount; 
            if (target.CurrentHealth > target.MaxHealth)
            {
                target.CurrentHealth = target.MaxHealth; 
            }
            Debug.Log($"{itemName} used on {target.MemberName}, healed for {healAmount} HP.");
        }
        else
        {
            Debug.Log($"{target.MemberName} is already at full health.");
        }
    }
}
