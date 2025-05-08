using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigo : MonoBehaviour
{
    public EnemyInfo enemyInfo;

    private void OnDestroy()
    {
        if (enemyInfo != null)
        {
            QuestManager questManager = FindFirstObjectByType<QuestManager>();
            if (questManager != null)
            {
                questManager.UpdateKillQuestObjective(enemyInfo);
            }
        }
    }
}
