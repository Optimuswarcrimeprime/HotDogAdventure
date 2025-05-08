using UnityEngine;

public class InteractableTalkObjective : MonoBehaviour
{
    [SerializeField] private string npcIdentifier = "ProfessorOak";
    [SerializeField] private GameObject interactPrompt;
    private bool playerInRange = false;

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.T))
        {
            QuestManager.instance.UpdateTalkQuestObjective(npcIdentifier);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (interactPrompt != null) interactPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (interactPrompt != null) interactPrompt.SetActive(false);
        }
    }
}