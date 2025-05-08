using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue/DialogueData")]
public class DialogueData : ScriptableObject
{
    [System.Serializable]
    public class DialogueLine
    {
        public string characterName;
        [TextArea(3, 10)]
        public string text;
    }

    [Header("Quest Not Started Dialogue")]
    public List<DialogueLine> notStartedDialogue;

    [Header("Quest In Progress Dialogue")]
    public List<DialogueLine> inProgressDialogue;

    [Header("Quest Completed Dialogue")]
    public List<DialogueLine> completedDialogue;

    [Header("Quest Rewarded Dialogue")]
    public List<DialogueLine> rewardedDialogue;
}