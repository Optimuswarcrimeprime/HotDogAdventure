using UnityEngine;

public class SceneTeleport : MonoBehaviour
{
    [Header("Teleport Settings")]
    public SceneTeleport destination; 

    [Header("Music Settings")]
    public GameObject currentMusicObject; 
    public GameObject nextMusicObject;    

    private bool isTeleporting = false; 

    private void OnTriggerEnter(Collider other)
    {
        if (isTeleporting) return; 

        if (other.CompareTag("Player"))
        {
            if (destination != null)
            {
                destination.isTeleporting = true;

                CharacterController cc = other.GetComponent<CharacterController>();
                if (cc) cc.enabled = false; 
                Vector3 playerOffset = other.transform.position - transform.position; 
                other.transform.position = destination.transform.position + playerOffset;
                if (cc) cc.enabled = true; 

                TeleportPartyMembers(playerOffset);
            }

            HandleMusicSwap();
        }
    }

    private void HandleMusicSwap()
    {
        if (currentMusicObject)
        {
            AudioSource currentAudio = currentMusicObject.GetComponent<AudioSource>();
            if (currentAudio)
            {
                currentAudio.playOnAwake = false; 
                currentMusicObject.SetActive(false); 
            }
        }

        if (nextMusicObject)
        {
            AudioSource nextAudio = nextMusicObject.GetComponent<AudioSource>();
            if (nextAudio)
            {
                nextAudio.playOnAwake = true;
                nextMusicObject.SetActive(true); 
                if (!nextAudio.isPlaying) nextAudio.Play(); 
            }
        }
    }

    private void TeleportPartyMembers(Vector3 playerOffset)
    {
        GameObject[] partyMembers = GameObject.FindGameObjectsWithTag("NPCJoinable");

        foreach (GameObject member in partyMembers)
        {
            MemberFollowAI followAI = member.GetComponent<MemberFollowAI>();
            if (followAI != null)
            {
                member.transform.position = destination.transform.position + playerOffset;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            isTeleporting = false;
    }
}