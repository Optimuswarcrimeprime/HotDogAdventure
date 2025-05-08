using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New Party Member")]
public class PartyMemberInfo : ScriptableObject
{
    public string MemberName;
    public int StartingLevel;
    public int BaseHealth;
    public int BaseStrength;
    public int BaseDefense;
    public int BaseMagic;
    public int BaseMagicDefense;
    public int BasePriority;
    public GameObject MemberBattleVisualPrefab;
    public GameObject MemberOverWorldVisualPrefab;
}
