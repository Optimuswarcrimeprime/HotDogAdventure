using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    [SerializeField] private PartyMemberInfo[] allMembers;
    [SerializeField] private List<PartyMember> currentParty;

    [SerializeField] private PartyMemberInfo defaultPartyMember;
    private Vector3 playerPosition;
    private static GameObject instance;

    [SerializeField] private List<PartyMember> deadParty = new List<PartyMember>();

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this.gameObject;
            AddMemberToPartyByName(defaultPartyMember.MemberName);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void AddMemberToPartyByName(string memberName)
    {
        for (int i = 0; i < allMembers.Length; i++)
        {
            if (allMembers[i].MemberName == memberName)
            {
                PartyMember newPartyMember = new PartyMember();
                newPartyMember.MemberName = allMembers[i].MemberName;
                newPartyMember.Level = allMembers[i].StartingLevel;
                newPartyMember.CurrentHealth = allMembers[i].BaseHealth;
                newPartyMember.MaxHealth = newPartyMember.CurrentHealth;
                newPartyMember.Strength = allMembers[i].BaseStrength;
                newPartyMember.Defense = allMembers[i].BaseDefense;
                newPartyMember.Magic = allMembers[i].BaseMagic;
                newPartyMember.MagicDefense = allMembers[i].BaseMagicDefense;
                newPartyMember.Priority = allMembers[i].BasePriority;
                newPartyMember.MemberBattleVisualPrefab = allMembers[i].MemberBattleVisualPrefab;
                newPartyMember.MemberOverworldVisualPrefab = allMembers[i].MemberOverWorldVisualPrefab;

                currentParty.Add(newPartyMember);
            }
        }
    }

    public List<PartyMember> GetAliveParty()
    {
        List<PartyMember> aliveParty = new List<PartyMember>();
        foreach (PartyMember member in currentParty)
        {
            if (member.CurrentHealth > 0)
                aliveParty.Add(member);
        }
        return aliveParty;
    }

    public List<PartyMember> GetDeadParty()
    {
        return deadParty;
    }

    public List<PartyMember> GetCurrentParty()
    {
        return currentParty;
    }

    public void MoveToDeadParty(PartyMember member)
    {
        if (currentParty.Contains(member))
        {
            currentParty.Remove(member);
            if (!deadParty.Contains(member))
                deadParty.Add(member);
        }
    }

    public void ReviveAllMembers()
    {
        foreach (PartyMember member in deadParty)
        {
            member.CurrentHealth = member.MaxHealth;
            currentParty.Add(member);
        }
        deadParty.Clear();

        foreach (PartyMember member in currentParty)
        {
            member.CurrentHealth = member.MaxHealth;
        }
    }

    public void SaveHealth(int partyMember, int health)
    {
        if (partyMember >= 0 && partyMember < currentParty.Count)
            currentParty[partyMember].CurrentHealth = health;
    }

    public void SetPosition(Vector3 position)
    {
        playerPosition = position;
    }

    public Vector3 GetPosition()
    {
        return playerPosition;
    }
}

[System.Serializable]
public class PartyMember
{
    public string MemberName;
    public int Level;
    public int CurrentHealth;
    public int MaxHealth;
    public int Strength;
    public int Defense;
    public int Magic;
    public int MagicDefense;
    public int Priority;
    public int CurrentExp;
    public int MaxExp;
    public GameObject MemberBattleVisualPrefab;
    public GameObject MemberOverworldVisualPrefab;
}