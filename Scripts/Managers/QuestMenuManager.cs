using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestMenuManager : MonoBehaviour
{
    public Canvas questMenuCanvas;
    public TextMeshProUGUI activeQuestsText;
    public TextMeshProUGUI completedQuestsText;

    public void Start()
    {
        questMenuCanvas.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            ToggleQuestMenu();
        }
    }

    private void ToggleQuestMenu()
    {
        if (questMenuCanvas != null)
        {
            questMenuCanvas.gameObject.SetActive(!questMenuCanvas.gameObject.activeSelf);
            if (questMenuCanvas.gameObject.activeSelf)
            {
                UpdateQuestMenu();
            }
        }
    }

    private void UpdateQuestMenu()
    {
        if (QuestManager.instance != null)
        {
            activeQuestsText.text = "Active Quests:\n";
            completedQuestsText.text = "Completed Quests:\n";

            foreach (var quest in QuestManager.instance.quests)
            {
                if (quest.isCompleted)
                {
                    completedQuestsText.text += $"{quest.questName}\n";
                }
                else
                {
                    if (quest.objectiveType == QuestData.ObjectiveType.Kill)
                    {
                        activeQuestsText.text += $"{quest.questName} - {quest.killCount}/{quest.killTarget} kills\n";
                    }
                    else if (quest.objectiveType == QuestData.ObjectiveType.Talk)
                    {
                        if (quest.currentObjectiveIndex < quest.targetNPCNames.Count)
                            activeQuestsText.text += $"{quest.questName} - Talk to {quest.targetNPCNames[quest.currentObjectiveIndex]}\n";
                        else
                            activeQuestsText.text += $"{quest.questName} - All objectives complete!\n";
                    }
                }
            }
        }
    }
}