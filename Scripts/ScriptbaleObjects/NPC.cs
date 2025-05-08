using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public string npcName;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            QuestManager questManager = FindFirstObjectByType<QuestManager>();
                if (questManager != null)
            {
                questManager.UpdateTalkQuestObjective(npcName);
            }
        }
    }
}
