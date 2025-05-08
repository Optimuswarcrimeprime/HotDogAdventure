using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestObjectiveTrigger : MonoBehaviour
{
    public string questName;
    public string objective;
    public QuestData questData;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
               FindFirstObjectByType<QuestManager>().StartQuest(questData);
        }
    }
}
