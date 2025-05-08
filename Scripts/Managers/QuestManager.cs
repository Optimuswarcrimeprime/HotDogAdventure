using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;
    public GameObject QuestCanvas;
    [SerializeField] private AudioSource audioSource; 
    public List<QuestData> quests;
    public TextMeshProUGUI questLogText;

    private GameObject questLogTextObject;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (QuestCanvas != null)
        {
            QuestCanvas.SetActive(false);
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void SetQuestLogText(TextMeshProUGUI questLogTextUI)
    {
        questLogText = questLogTextUI;
        questLogTextObject = questLogText.gameObject;
        DontDestroyOnLoad(questLogTextObject);
        UpdateQuestLogUI();
    }

    public void StartQuest(QuestData quest)
    {
        if (!quests.Contains(quest))
        {
            quests.Add(quest);
            Debug.Log($"Quest '{quest.questName}' started.");

            if (QuestCanvas != null)
            {
                StartCoroutine(ShowQuestCanvas());
            }

            if (questLogText != null)
            {
                UpdateQuestLogUI();
            }
        }
        else
        {
            Debug.LogWarning($"Quest '{quest.questName}' is already active or completed!");
        }
    }

    private IEnumerator ShowQuestCanvas()
    {
        QuestCanvas.SetActive(true);

        if (audioSource != null)
        {
            audioSource.Play();
        }

        if (audioSource != null && audioSource.clip != null)
        {
            yield return new WaitForSeconds(audioSource.clip.length);
        }
        else
        {
            yield return new WaitForSeconds(2f);
        }

        QuestCanvas.SetActive(false);
    }

    public void CompleteQuest(string questName)
    {
        foreach (var quest in quests)
        {
            if (quest.questName == questName && quest.currentState == QuestData.QuestState.Completed)
            {
                quest.isCompleted = true;
                quest.currentState = QuestData.QuestState.Rewarded;

                Debug.Log($"Quest '{questName}' is now rewarded!");

                if (quest.unlockableObject != null)
                {
                    quest.unlockableObject.SetActive(true);
                    Debug.Log($"Unlocked object for quest '{questName}'!");
                }

                foreach (var rewardItem in quest.rewardItems)
                {
                    AddItemToInventory(rewardItem);
                }

                if (questLogText != null)
                {
                    UpdateQuestLogUI();
                }

                return;
            }
        }

        Debug.LogWarning($"Quest '{questName}' not found or already rewarded!");
    }

    private void AddItemToInventory(Item rewardItem)
    {
        Inventory.instance.AddItem(rewardItem);

        ItemPopUpManager.instance.ShowItemPopUp(rewardItem);
    }

    public void UpdateKillQuestObjective(EnemyInfo enemyInfo)
    {
        foreach (var quest in quests)
        {
            if (quest.objectiveType == QuestData.ObjectiveType.Kill && quest.targetEnemy == enemyInfo && quest.currentState == QuestData.QuestState.InProgress)
            {
                quest.killCount++;

                if (quest.killCount >= quest.killTarget)
                {
                    quest.currentState = QuestData.QuestState.ReadyToComplete;
                    Debug.Log($"Quest '{quest.questName}' is ready to complete. Talk to the NPC to finish it.");
                }

                if (questLogText != null)
                {
                    UpdateQuestLogUI();
                }

                return;
            }
        }

        Debug.LogWarning($"Quest for killing '{enemyInfo.EnemyName}' not found or already completed!");
    }

    public void UpdateTalkQuestObjective(string npcName)
    {
        foreach (var quest in quests)
        {
            if (quest.objectiveType == QuestData.ObjectiveType.Talk && !quest.isCompleted)
            {
                if (quest.currentObjectiveIndex < quest.targetNPCNames.Count && npcName == quest.targetNPCNames[quest.currentObjectiveIndex])
                {
                    quest.currentObjectiveIndex++;
                    if (quest.currentObjectiveIndex >= quest.targetNPCNames.Count)
                    {
                        quest.currentState = QuestData.QuestState.Completed;
                        CompleteQuest(quest.questName);
                    }
                    if (questLogText != null)
                    {
                        UpdateQuestLogUI();
                    }
                    return;
                }
            }
        }
    }

    private void UpdateQuestLogUI()
    {
        if (questLogText != null)
        {
            questLogText.text = "";
            foreach (var quest in quests)
            {
                string status = quest.isCompleted ? "Completed" : "In Progress";
                if (quest.objectiveType == QuestData.ObjectiveType.Kill)
                {
                    status += $" ({quest.killCount}/{quest.killTarget} kills)";
                }
                else if (quest.objectiveType == QuestData.ObjectiveType.Talk)
                {
                    if (!quest.isCompleted && quest.currentObjectiveIndex < quest.targetNPCNames.Count)
                        status += $" (Talk to {quest.targetNPCNames[quest.currentObjectiveIndex]})";
                    else
                        status += " (All objectives complete!)";
                }
                questLogText.text += $"{quest.questName} - {status}\n";
            }
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateQuestLogUI();

        foreach (var quest in quests)
        {
            if (quest.isCompleted && quest.unlockableObject != null)
            {
                quest.unlockableObject.SetActive(true);
            }
        }
    }
}