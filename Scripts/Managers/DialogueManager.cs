using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public GameObject dialogueUI;
    public GameObject FunnyCanvas;

    private Queue<DialogueData.DialogueLine> lines;
    private bool isDialogueActive;

    [SerializeField] private bool autoProgressDialogue = false; 
    [SerializeField] private float autoProgressDelay = 2f; 

    public delegate void DialogueEndedHandler();
    public event DialogueEndedHandler OnDialogueEnded;

    void Start()
    {
        lines = new Queue<DialogueData.DialogueLine>();
        dialogueUI.SetActive(false);
        FunnyCanvas.SetActive(true);
    }

    public void StartDialogue(DialogueData dialogueData, QuestData.QuestState questState)
    {
        isDialogueActive = true;
        dialogueUI.SetActive(true);
        lines.Clear();

        List<DialogueData.DialogueLine> selectedDialogue = null;
        switch (questState)
        {
            case QuestData.QuestState.NotStarted:
                selectedDialogue = dialogueData.notStartedDialogue;
                break;
            case QuestData.QuestState.InProgress:
                selectedDialogue = dialogueData.inProgressDialogue;
                break;
            case QuestData.QuestState.Completed:
                selectedDialogue = dialogueData.completedDialogue;
                break;
            case QuestData.QuestState.Rewarded:
                selectedDialogue = dialogueData.rewardedDialogue;
                break;
        }

        if (selectedDialogue == null || selectedDialogue.Count == 0)
        {
            Debug.LogWarning("No dialogue available for the current quest state!");
            EndDialogue();
            return;
        }

        foreach (var line in selectedDialogue)
        {
            lines.Enqueue(line);
        }

        DisplayNextLine();
    }

    public void DisplayNextLine()
    {
        if (lines.Count == 0)
        {
            EndDialogue();
            return;
        }

        var line = lines.Dequeue();
        nameText.text = line.characterName;
        StopAllCoroutines();
        StartCoroutine(TypeSentence(line.text));
    }

    private IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null; 
        }

        if (autoProgressDialogue)
        {
            yield return new WaitForSeconds(autoProgressDelay);
            DisplayNextLine();
        }
    }

    private void EndDialogue()
    {
        dialogueUI.SetActive(false);
        isDialogueActive = false;

        OnDialogueEnded?.Invoke();
    }

    void Update()
    {
        if (isDialogueActive && Input.GetKeyDown(KeyCode.Y)) 
        {
            DisplayNextLine();
        }
    }

    public bool IsDialogueActive()
    {
        return isDialogueActive;
    }
}