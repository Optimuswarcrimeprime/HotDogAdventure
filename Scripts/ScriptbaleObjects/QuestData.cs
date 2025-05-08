using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewQuest", menuName = "Quest/QuestData")]
public class QuestData : ScriptableObject
{
    [Header("Quest Information")]
    public string questName; 
    public string description;
    public bool isCompleted; 

    [Header("Quest Objectives")]
    public string[] objectives; 

    public enum ObjectiveType { Kill, Talk }
    public ObjectiveType objectiveType;

    public EnemyInfo targetEnemy; 
    public int killCount;
    public int killTarget; 

    public string targetNPCName;
    public List<string> targetNPCNames; 
    public int currentObjectiveIndex; 

    [Header("Quest State")]
    public QuestState currentState = QuestState.NotStarted;

    public enum QuestState
    {
        NotStarted,
        InProgress,
        ReadyToComplete, 
        Completed,
        Rewarded
    }

    [Header("Quest Rewards")]
    public List<Item> rewardItems; 

    [Header("Optional Unlockable Object")]
    public GameObject unlockableObject; 
}