using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private EnemyInfo[] allEnemies;
    [SerializeField] private List<Enemy> currentEnemies;

    private static GameObject instance;

    private const float LEVEL_MODIFIER = 0.5f;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this.gameObject;
        }
        //GenerateEnemyByName("Kecheob", 5);
        DontDestroyOnLoad(gameObject);
    }

    public void GenerateEnemiesByEncounter(Encounter[] encounters, int maxNumEnemies)
    {
        currentEnemies.Clear();
        int numEnemies = Random.Range(1, maxNumEnemies + 1);

        for (int i = 0; i < numEnemies; i++)
        {
            Encounter tempEncounter = encounters[Random.Range(0, encounters.Length)];
            int level = Random.Range(tempEncounter.LevelMin, tempEncounter.LevelMAx + 1);
            GenerateEnemyByName(tempEncounter.Enemy.EnemyName, level);
        }
    }

    private void GenerateEnemyByName(string enemyName, int level)
    {
        for (int i = 0; i < allEnemies.Length; i++)
        {
            if (enemyName == allEnemies[i].EnemyName)
            {
                Enemy newEnemy = new Enemy();

                newEnemy.EnemyName = allEnemies[i].EnemyName;
                newEnemy.Level = level;
                float levelModifier = (LEVEL_MODIFIER * newEnemy.Level);

                newEnemy.MaxHealth = Mathf.RoundToInt(allEnemies[i].BaseHealth + (allEnemies[i].BaseHealth * levelModifier));
                newEnemy.CurrentHealth = newEnemy.MaxHealth;
                newEnemy.Strength = Mathf.RoundToInt(allEnemies[i].BaseStrength + (allEnemies[i].BaseStrength * levelModifier));
                newEnemy.Defense = Mathf.RoundToInt(allEnemies[i].BaseDefense + (allEnemies[i].BaseDefense * levelModifier));
                newEnemy.Magic = Mathf.RoundToInt(allEnemies[i].BaseMagic + (allEnemies[i].BaseMagic * levelModifier));
                newEnemy.MagicDefense = Mathf.RoundToInt(allEnemies[i].BaseMagicDefense + (allEnemies[i].BaseMagicDefense * levelModifier));
                newEnemy.Priority = Mathf.RoundToInt(allEnemies[i].BasePriority + (allEnemies[i].BasePriority * levelModifier));
                newEnemy.EnemyVisualPrefab = allEnemies[i].EnemyVisualPrefab;

                currentEnemies.Add(newEnemy);
            }
        }
    }

    public List<Enemy> GetCurrentEnemies()
    {
        return currentEnemies;
    }
}

[System.Serializable]
public class Enemy
{
    public string EnemyName;
    public int Level;
    public int CurrentHealth;
    public int MaxHealth;
    public int Strength;
    public int Defense;
    public int Magic;
    public int MagicDefense;
    public int Priority;
    public GameObject EnemyVisualPrefab;
}
