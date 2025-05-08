using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterSystem2 : MonoBehaviour
{
    [SerializeField] private Encounter2[] enemiesInScene; 
    [SerializeField] private int maxNumEnemies; 

    private EnemyManager enemyManager;
    private List<EncounterSystem> disabledEncounterSystems = new List<EncounterSystem>(); 

    void Start()
    {
        enemyManager = GameObject.FindFirstObjectByType<EnemyManager>();
        if (enemyManager == null)
        {
            Debug.LogError("EncounterSystem2: EnemyManager not found in the scene!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("EncounterSystem2: Player entered the trigger.");

            DisableEncounterSystem1();

            TriggerEncounter();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("EncounterSystem2: Player exited the trigger.");

            EnableEncounterSystem1();
        }
    }

    private void TriggerEncounter()
    {
        if (enemyManager == null)
        {
            Debug.LogError("EncounterSystem2: EnemyManager is not initialized!");
            return;
        }

        if (enemiesInScene == null || enemiesInScene.Length == 0)
        {
            Debug.LogError("EncounterSystem2: No enemies defined for this trigger!");
            return;
        }

        Debug.Log($"EncounterSystem2: Triggering encounter with {enemiesInScene.Length} possible enemies.");

        Encounter[] guaranteedEnemies = GetUniqueAndRandomizedEnemies(enemiesInScene, maxNumEnemies);
        enemyManager.GenerateEnemiesByEncounter(guaranteedEnemies, guaranteedEnemies.Length);
    }

    private void DisableEncounterSystem1()
    {
        EncounterSystem[] encounterSystems = FindObjectsByType<EncounterSystem>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        foreach (EncounterSystem encounterSystem in encounterSystems)
        {
            if (encounterSystem.enabled)
            {
                encounterSystem.enabled = false; 
                disabledEncounterSystems.Add(encounterSystem); 
                Debug.Log($"EncounterSystem2: Disabled EncounterSystem on {encounterSystem.gameObject.name}.");
            }
        }
    }

    private void EnableEncounterSystem1()
    {
        
        foreach (EncounterSystem encounterSystem in disabledEncounterSystems)
        {
            if (encounterSystem != null)
            {
                encounterSystem.enabled = true;
                Debug.Log($"EncounterSystem2: Re-enabled EncounterSystem on {encounterSystem.gameObject.name}.");
            }
        }

        disabledEncounterSystems.Clear();
    }

    private Encounter[] GetUniqueAndRandomizedEnemies(Encounter2[] enemies, int maxNum)
    {
        List<Encounter> uniqueEnemies = new List<Encounter>();
        foreach (var enemy in enemies)
        {
            uniqueEnemies.Add(new Encounter
            {
                Enemy = enemy.Enemy,
                LevelMin = enemy.LevelMin,
                LevelMAx = enemy.LevelMAx
            });
        }

        ShuffleList(uniqueEnemies);

        while (uniqueEnemies.Count < maxNum)
        {
            var randomEnemy = enemies[UnityEngine.Random.Range(0, enemies.Length)];
            var newEncounter = new Encounter
            {
                Enemy = randomEnemy.Enemy,
                LevelMin = randomEnemy.LevelMin,
                LevelMAx = randomEnemy.LevelMAx
            };

            if (!uniqueEnemies.Exists(e => e.Enemy == newEncounter.Enemy))
            {
                uniqueEnemies.Add(newEncounter);
            }
        }

        if (uniqueEnemies.Count > maxNum)
        {
            uniqueEnemies = uniqueEnemies.GetRange(0, maxNum);
        }

        return uniqueEnemies.ToArray();
    }

    private void ShuffleList<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = UnityEngine.Random.Range(0, i + 1);
            T temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}

[System.Serializable]
public class Encounter2
{
    public EnemyInfo Enemy; 
    public int LevelMin;  
    public int LevelMAx;
}