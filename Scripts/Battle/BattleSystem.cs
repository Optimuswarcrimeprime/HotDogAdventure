using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class BattleSystem : MonoBehaviour
{

    [SerializeField] private enum BattleState { Start, Selection, Battle, Won, Lost, Run }

    [Header("Battle State")]
    [SerializeField] private BattleState state;

    [Header("Spawn Points")]
    [SerializeField] private Transform[] partySpawnPoints;
    [SerializeField] private Transform[] enemySpawnPoints;

    [Header("Battlers")]
    [SerializeField] private List<BattleEntities> allBattlers = new List<BattleEntities>();
    [SerializeField] private List<BattleEntities> enemyBattlers = new List<BattleEntities>();
    [SerializeField] private List<BattleEntities> playerBattlers = new List<BattleEntities>();

    [Header("UI")]
    [SerializeField] private GameObject[] enemySelectionButtons;
    [SerializeField] private GameObject battleMenu;
    [SerializeField] private GameObject enemySelectionMenu;
    [SerializeField] private TextMeshProUGUI actionText;
    [SerializeField] private GameObject bottomTextPopUp;
    [SerializeField] private TextMeshProUGUI bottomText;

    private PartyManager partyManager;
    private EnemyManager enemyManager;
    private int currentPlayer;

    private const string ACTION_MESSAGE = "'s Action:";
    private const string WIN_MESSAGE = "by Brandons grace, your party has won the battle";
    private const string LOSE_MESSAGE = "by Brandons sin, your party has suffered a crushing defeat";
    private const string SUCCESSFULLY_RAN_MESSAGE = "You got away!!";
    private const string UNSUCCESSFULLY_RAN_MESSAGE = "Nah";
    private const int TURN_DURATION = 2;
    private const int RUN_CHANCE = 50;
    private const string OVERWORLD_SCENE = "OverWorld";

    void Start()
    {
        partyManager = GameObject.FindFirstObjectByType<PartyManager>();
        enemyManager = GameObject.FindFirstObjectByType<EnemyManager>();

        CreatePartyEntities();
        CreateEnemyEntities();
        ShowBattleMenu();
        DetermineBattleOrder();
    }

    private IEnumerator BattleRoutine()
    {
        enemySelectionMenu.SetActive(false); 
        state = BattleState.Battle; 
        bottomTextPopUp.SetActive(true);

        for (int i = 0; i < allBattlers.Count; i++)
        {
            if (state == BattleState.Battle)
            {
                switch (allBattlers[i].BattleAction)
                {
                case BattleEntities.Action.Attack:
                    yield return StartCoroutine(AttackRoutine(i));
                    break;
                case BattleEntities.Action.Magic: 
                    yield return StartCoroutine(MagicRoutine(i));
                    break;
                case BattleEntities.Action.Run: 
                    yield return StartCoroutine(RunRoutine());
                    break;
                default: 
                    Debug.Log("Erorr, Brandon is White");
                    break;
                }
            }
        }

        if (state == BattleState.Battle)
        {
            bottomTextPopUp.SetActive(false);
            currentPlayer = 0;
            ShowBattleMenu();
        }
    }

    private IEnumerator AttackRoutine(int i)
    {
       
        if (allBattlers[i].IsPlayer == true)
        {
            BattleEntities currentAttacker = allBattlers[i];
            if (allBattlers[currentAttacker.Target].IsPlayer == true || currentAttacker.Target >= allBattlers.Count)
            {
                currentAttacker.SetTarget(GetRandomEnemy());
            }
            
            BattleEntities currentTarget = allBattlers[currentAttacker.Target];
            AttackAction(currentAttacker, currentTarget); 
            yield return new WaitForSeconds(TURN_DURATION);

            if (currentTarget.CurrentHealth <= 0)
            {
                bottomText.text = string.Format("{0} defeated {1}", currentAttacker.Name, currentTarget.Name);
                yield return new WaitForSeconds(TURN_DURATION); 
                enemyBattlers.Remove(currentTarget);
                allBattlers.Remove(currentTarget);

                if (enemyBattlers.Count <= 0)
                {
                    state = BattleState.Won;
                    bottomText.text = WIN_MESSAGE;
                    yield return new WaitForSeconds(TURN_DURATION); 
                    SceneManager.LoadScene(OVERWORLD_SCENE);
                }
            }

        }

        if (i< allBattlers.Count && allBattlers[i].IsPlayer == false)
        {
            BattleEntities currentAttacker = allBattlers[i];
            currentAttacker.SetTarget(GetRandomPartyMember()); 
            BattleEntities currentTarget = allBattlers[currentAttacker.Target];

            AttackAction(currentAttacker, currentTarget);
            yield return new WaitForSeconds(TURN_DURATION); 

            if (currentTarget.CurrentHealth <= 0)
            {
                bottomText.text = string.Format("{0} defeated {1}", currentAttacker.Name, currentTarget.Name); 
                yield return new WaitForSeconds(TURN_DURATION);
                playerBattlers.Remove(currentTarget);
                allBattlers.Remove(currentTarget);

                if (playerBattlers.Count <= 0) 
                {
                    state = BattleState.Lost;
                    bottomText.text = LOSE_MESSAGE;
                    yield return new WaitForSeconds(TURN_DURATION);
                    Debug.Log("Game Over");
                }
            }

        }
    }

    private IEnumerator MagicRoutine(int i)
    {
        if (allBattlers[i].IsPlayer == true)
        {
            BattleEntities currentAttacker = allBattlers[i];
            if (allBattlers[currentAttacker.Target].IsPlayer == true || currentAttacker.Target >= allBattlers.Count)
            {
                currentAttacker.SetTarget(GetRandomEnemy());
            }
            
            BattleEntities currentTarget = allBattlers[currentAttacker.Target];
            MagicAction(currentAttacker, currentTarget);
            yield return new WaitForSeconds(TURN_DURATION);

            // kill the enemy
            if (currentTarget.CurrentHealth <= 0)
            {
                bottomText.text = string.Format("{0} defeated {1}", currentAttacker.Name, currentTarget.Name);
                yield return new WaitForSeconds(TURN_DURATION); 
                enemyBattlers.Remove(currentTarget);
                allBattlers.Remove(currentTarget);

                if (enemyBattlers.Count <= 0)
                {
                    state = BattleState.Won;
                    bottomText.text = WIN_MESSAGE;
                    yield return new WaitForSeconds(TURN_DURATION); 
                    SceneManager.LoadScene(OVERWORLD_SCENE);
                }
            }
        }

        if (i< allBattlers.Count && allBattlers[i].IsPlayer == false)
        {
            BattleEntities currentAttacker = allBattlers[i];
            currentAttacker.SetTarget(GetRandomPartyMember()); 
            BattleEntities currentTarget = allBattlers[currentAttacker.Target];

            MagicAction(currentAttacker, currentTarget);
            yield return new WaitForSeconds(TURN_DURATION); 

            if (currentTarget.CurrentHealth <= 0)
            {
                bottomText.text = string.Format("{0} defeated {1}", currentAttacker.Name, currentTarget.Name); 
                yield return new WaitForSeconds(TURN_DURATION); 
                playerBattlers.Remove(currentTarget);
                allBattlers.Remove(currentTarget);

                if (playerBattlers.Count <= 0) 
                {
                    state = BattleState.Lost; 
                    bottomText.text = LOSE_MESSAGE;
                    yield return new WaitForSeconds(TURN_DURATION);
                    Debug.Log("Game Over");
                }
            }

        }
    }

    private IEnumerator RunRoutine()
    {
        if (state == BattleState.Battle)
        {
            if(Random.Range(1,101) >= RUN_CHANCE)
            {

                bottomText.text = SUCCESSFULLY_RAN_MESSAGE; 
                state = BattleState.Run;
                allBattlers.Clear();
                yield return new WaitForSeconds(TURN_DURATION); 
                SceneManager.LoadScene(OVERWORLD_SCENE);
                yield break;
            }      
            else
            {

                 bottomText.text = UNSUCCESSFULLY_RAN_MESSAGE;
                yield return new WaitForSeconds(TURN_DURATION);
            }   
        }
    }

    private void CreatePartyEntities()
    {

        List<PartyMember> currentParty = new List<PartyMember>();
        currentParty = partyManager.GetAliveParty();

        for (int i = 0; i < currentParty.Count; i++)
        {
            BattleEntities tempEntity = new BattleEntities();

            tempEntity.SetEntityValues(currentParty[i].MemberName, currentParty[i].CurrentHealth, currentParty[i].MaxHealth,
            currentParty[i].Priority, currentParty[i].Strength, currentParty[i].Defense, currentParty[i].Magic, currentParty[i].MagicDefense, currentParty[i].Level, true);

            BattleVisuals tempBattleVisuals = Instantiate(currentParty[i].MemberBattleVisualPrefab,
            partySpawnPoints[i].position, Quaternion.identity).GetComponent<BattleVisuals>();
            tempBattleVisuals.SetStaringValues(currentParty[i].CurrentHealth, currentParty[i].MaxHealth, currentParty[i].Level);
            tempEntity.BattleVisuals = tempBattleVisuals;

            allBattlers.Add(tempEntity);
            playerBattlers.Add(tempEntity);
        }

    }

    private void CreateEnemyEntities()
    {
        List<Enemy> currentEnemies = new List<Enemy>();
        currentEnemies = enemyManager.GetCurrentEnemies();

        for (int i = 0; i < currentEnemies.Count; i++)
        {
            BattleEntities tempEntity = new BattleEntities();

            tempEntity.SetEntityValues(currentEnemies[i].EnemyName, currentEnemies[i].CurrentHealth, currentEnemies[i].MaxHealth,
            currentEnemies[i].Priority, currentEnemies[i].Strength, currentEnemies[i].Defense, currentEnemies[i].Magic, 
            currentEnemies[i].MagicDefense, currentEnemies[i].Level, false);

            BattleVisuals tempBattleVisuals = Instantiate(currentEnemies[i].EnemyVisualPrefab,
            enemySpawnPoints[i].position, Quaternion.identity).GetComponent<BattleVisuals>();
            tempBattleVisuals.SetStaringValues(currentEnemies[i].CurrentHealth, currentEnemies[i].MaxHealth, currentEnemies[i].Level);
            tempEntity.BattleVisuals = tempBattleVisuals;

            tempEntity.BattleAction = (Random.value > 0.5f) ? BattleEntities.Action.Attack : BattleEntities.Action.Magic;

            allBattlers.Add(tempEntity);
            enemyBattlers.Add(tempEntity);
        }
    }

    public void ShowBattleMenu()
    {
        actionText.text = playerBattlers[currentPlayer].Name + ACTION_MESSAGE;
        battleMenu.SetActive(true);
    }

    public void ShowEnemySelectionMenu()
    {
        battleMenu.SetActive(false);
        SetEnemySelectionButtons();
        enemySelectionMenu.SetActive(true);
    }

    public void SetEnemySelectionButtons()
    {

        for (int i = 0; i < enemySelectionButtons.Length; i++)
        {
            enemySelectionButtons[i].SetActive(false);
        }

        for (int j = 0; j < enemyBattlers.Count; j++)
        {
            enemySelectionButtons[j].SetActive(true);
            enemySelectionButtons[j].GetComponentInChildren<TextMeshProUGUI>().text = enemyBattlers[j].Name;
        }
    }

    public void SelectEnemy(int currentEnemy)
    {
        BattleEntities currentPlayerEntity = playerBattlers[currentPlayer];  
        currentPlayerEntity.SetTarget(allBattlers.IndexOf(enemyBattlers[currentEnemy]));

        currentPlayer++;    

        if (currentPlayer >= playerBattlers.Count) 
        {
            StartCoroutine(BattleRoutine());
        }
        else
        {
            enemySelectionMenu.SetActive(false); 
            ShowBattleMenu();
        }
    }

    private void AttackAction(BattleEntities currentAttacker, BattleEntities currentTarget)
    {
        int damage = currentAttacker.Strength;
        int finalDamage = currentTarget.TakePhysicalDamage(damage); 
        currentAttacker.BattleVisuals.PlayAttackAnimation();
        currentTarget.BattleVisuals.PlayHitAnimation(); 
        currentTarget.UpdateUI(); 
        bottomText.text = string.Format("{0} attacks {1} for {2} damage", currentAttacker.Name, currentTarget.Name, finalDamage); 

    }

    private void MagicAction(BattleEntities currentAttacker, BattleEntities currentTarget)
    {
        int damage = currentAttacker.Magic;
        int finalDamage = currentTarget.TakeMagicDamage(damage);
        currentAttacker.BattleVisuals.PlayAttackAnimation();
        currentTarget.BattleVisuals.PlayHitAnimation(); 
        currentTarget.UpdateUI(); 
        bottomText.text = string.Format("{0} bibidi babibi boo's {1} for {2} damage", currentAttacker.Name, currentTarget.Name, finalDamage); 
        SaveHealth();

    }

    private int GetRandomPartyMember()
    {
        List<int> partyMembers = new List<int>();
        for (int i = 0; i < allBattlers.Count; i++)
        {
            if (allBattlers[i].IsPlayer == true)
            {
                partyMembers.Add(i);
            }
        }
        return partyMembers[Random.Range(0, partyMembers.Count)]; 
    }

    private int GetRandomEnemy()
    {
        List<int> enemies = new List<int>();
        for (int i = 0; i < allBattlers.Count; i++)
        {
            if (allBattlers[i].IsPlayer == false)
            {
                enemies.Add(i);
            }
        }
        return enemies[Random.Range(0, enemies.Count)];
    }

    private void SaveHealth()
    {
        for (int i = 0; i < playerBattlers.Count; i++)
        {
             partyManager.SaveHealth(i, playerBattlers[i].CurrentHealth);
        }
    }

    private void DetermineBattleOrder()
    {
        allBattlers.Sort((bi1, bi2) => -bi1.Priority.CompareTo(bi2.Priority)); 
    }

    public void SelectRunAction()
    {
        state = BattleState.Selection;
        BattleEntities currentPlayerEntity = playerBattlers[currentPlayer];

        currentPlayerEntity.BattleAction = BattleEntities.Action.Run; 

        battleMenu.SetActive(false);
        currentPlayer++;

        if (currentPlayer >= playerBattlers.Count) 
        {
            StartCoroutine(BattleRoutine()); 
        }
        else
        {
            enemySelectionMenu.SetActive(false); 
            ShowBattleMenu();
        }
    }

    public void OnAttackButtonClicked()
    {
        BattleEntities currentPlayerEntity = playerBattlers[currentPlayer];
        currentPlayerEntity.BattleAction = BattleEntities.Action.Attack;
        ShowEnemySelectionMenu();
    }

    public void OnMagicButtonClicked()
    {
        BattleEntities currentPlayerEntity = playerBattlers[currentPlayer];
        currentPlayerEntity.BattleAction = BattleEntities.Action.Magic;
        ShowEnemySelectionMenu();
    }



}

    [System.Serializable]
    public class BattleEntities
    {
        public enum Action { Attack, Magic, Run, Item }
        public Action BattleAction;
        public string Name;
        public int CurrentHealth;
        public int MaxHealth;
        public int Priority;
        public int Strength;
        public int Defense;
        public int Magic;
        public int MagicDefense;
        public int Level;
        public bool IsPlayer;
        public BattleVisuals BattleVisuals;
        public int Target;



        public void SetEntityValues(string name, int currentHealth, int maxHealth, int priority, int strength, int defense, int magic, int magicDefense, int level, bool isPlayer)
        {
            Name = name;
            CurrentHealth = currentHealth;
            MaxHealth = maxHealth;
            Priority = priority;
            Strength = strength;
            Defense = defense;
            Magic = magic;
            MagicDefense = magicDefense;
            Level = level;
            IsPlayer = isPlayer;
        }

        public void SetTarget(int target)
        {
            Target = target;
        }

        public void UpdateUI()
        {
            BattleVisuals.ChangeHealth(CurrentHealth);
        }

        public int TakePhysicalDamage(int damage)
        {
            int finalDamage = Mathf.Max(damage - Defense, 0); 
            CurrentHealth -= finalDamage;
            return finalDamage;
        }

        public int TakeMagicDamage(int damage)
        {
            int finalDamage = Mathf.Max(damage - MagicDefense, 0); 
            CurrentHealth -= finalDamage;
            return finalDamage;
        }

    }

