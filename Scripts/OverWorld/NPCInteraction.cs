using System.Collections.Generic;
using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    [SerializeField] private DialogueData dialogueData;
    [SerializeField] private QuestData questData;
    [SerializeField] private GameObject interactPrompt;
    [SerializeField] private DialogueManager dialogueManager; 
    [SerializeField] private GameObject teleporter; 
    [SerializeField] private string npcIdentifier = "ProfessorOak";

    private bool playerInRange = false;

    private void Start()
    {
        if (teleporter != null)
            teleporter.SetActive(false); 

        if (dialogueManager != null)
            dialogueManager.OnDialogueEnded += OnDialogueFinished; 
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.T) && !dialogueManager.IsDialogueActive())
        {
            InteractWithNPC();
        }
    }

    public void ShowInteractPrompt(bool showPrompt)
    {
        interactPrompt.SetActive(showPrompt);
    }

    private void InteractWithNPC()
    {
        if (dialogueManager == null)
        {
            Debug.LogError("DialogueManager is not assigned in NPCInteraction!");
            return;
        }

        switch (questData.currentState)
        {
            case QuestData.QuestState.NotStarted:
                dialogueManager.StartDialogue(dialogueData, QuestData.QuestState.NotStarted);
                break;

            case QuestData.QuestState.InProgress:
                dialogueManager.StartDialogue(dialogueData, QuestData.QuestState.InProgress);
                break;

            case QuestData.QuestState.ReadyToComplete:
                dialogueManager.StartDialogue(dialogueData, QuestData.QuestState.Completed); 
                questData.currentState = QuestData.QuestState.Completed; 
                QuestManager.instance.CompleteQuest(questData.questName); 
                break;

            case QuestData.QuestState.Completed:
                dialogueManager.StartDialogue(dialogueData, QuestData.QuestState.Completed);
                break;

            case QuestData.QuestState.Rewarded:
                dialogueManager.StartDialogue(dialogueData, QuestData.QuestState.Rewarded);
                if (teleporter != null)
                    teleporter.SetActive(true);
                break;
        }
    }

    private void OnDialogueFinished()
    {
        if (questData.currentState == QuestData.QuestState.NotStarted)
        {
            questData.currentState = QuestData.QuestState.InProgress;
            QuestManager.instance.StartQuest(questData); 
            Debug.Log($"Quest '{questData.questName}' has been started!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            ShowInteractPrompt(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            ShowInteractPrompt(false);
        }
    }

    private void OnDestroy()
    {
        if (dialogueManager != null)
            dialogueManager.OnDialogueEnded -= OnDialogueFinished; 
    }
}