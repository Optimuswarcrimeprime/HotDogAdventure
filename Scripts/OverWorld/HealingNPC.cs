using System.Collections;
using UnityEngine;
using TMPro;

public class HealingNPCInteraction : MonoBehaviour
{
    [Header("UI & Feedback")]
    [SerializeField] private GameObject interactPrompt;
    [SerializeField] private TextMeshProUGUI healTextUI; 
    [SerializeField] private string healMessage = "Your party has been fully healed!";
    [SerializeField] private float messageDisplayTime = 2f;

    [Header("Audio")]
    [SerializeField] private AudioSource healAudioSource; 

    private bool playerInRange = false;
    private PartyManager partyManager;

    private void Start()
    {
        partyManager = FindFirstObjectByType<PartyManager>();
        if (interactPrompt != null)
            interactPrompt.SetActive(false);
        if (healTextUI != null)
            healTextUI.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.T))
        {
            HealParty();
        }
    }

    private void HealParty()
    {
        if (partyManager != null)
        {
            partyManager.ReviveAllMembers();

            if (healAudioSource != null)
            {
                healAudioSource.Stop();
                healAudioSource.Play();
            }

            if (healTextUI != null)
            {
                healTextUI.text = healMessage;
                healTextUI.gameObject.SetActive(true);
                StopAllCoroutines(); 
                StartCoroutine(HideHealTextAfterDelay());
            }
        }
        else
        {
            Debug.LogWarning("PartyManager not found for healing!");
        }
    }

    private IEnumerator HideHealTextAfterDelay()
    {
        yield return new WaitForSeconds(messageDisplayTime);
        if (healTextUI != null)
            healTextUI.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (interactPrompt != null)
                interactPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (interactPrompt != null)
                interactPrompt.SetActive(false);
        }
    }
}