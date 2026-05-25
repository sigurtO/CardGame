using UnityEngine;

public class BattleAnalyticsManager : MonoBehaviour
{
    [Header("Data Backpack")]
    [SerializeField] private CurrentRunState runState;

    private bool tookFleshDamageThisBattle = false;
    private int damageDealtThisTurn = 0;
    private bool isTrackingTime = false;

    private void OnEnable()
    {
        Enemy.OnEnemyDied += HandleEnemyKill;
        EnemyManager.OnBattleWon += HandleBattleWon;
        Player.OnPlayerTookDamage += HandlePlayerDamage;
        Enemy.OnEnemyTookDamage += HandleEnemyTookDamage;
        TurnManager.OnPlayerTurnEnded += EvaluateTurnDamage;
        Player.OnDeath += HandlePlayerDeath;
    }

    private void OnDisable()
    {
        Enemy.OnEnemyDied -= HandleEnemyKill;
        EnemyManager.OnBattleWon -= HandleBattleWon;
        Player.OnPlayerTookDamage -= HandlePlayerDamage;
        Enemy.OnEnemyTookDamage -= HandleEnemyTookDamage;
        TurnManager.OnPlayerTurnEnded -= EvaluateTurnDamage;
        Player.OnDeath -= HandlePlayerDeath;

    }

    private void Start()
    {
        // Start the clock the moment the battle scene finishes loading!
        isTrackingTime = true;
    }

    private void Update()
    {
        // Only tick the clock if the battle is actively happening
        if (isTrackingTime && runState != null)
        {
            runState.runStats.timeSpentInBattle += Time.deltaTime;
        }
    }

    private void HandleEnemyKill(Enemy deadEnemy)
    {
        runState.runStats.enemiesKilled++;
    }
    private void HandleBattleWon()
    {
        isTrackingTime = false; //stop clock battle is over

        runState.runStats.encountersSurvived++;

        if (!tookFleshDamageThisBattle)
        {
            runState.runStats.perfectFights++;
        }
    }

    private void HandlePlayerDamage(int fleshDamage, int shieldAbsorbed)
    {
        runState.runStats.totalDamageTaken += fleshDamage; //add numbers to our runstate
        runState.runStats.damageAbsorbed += shieldAbsorbed;

        
        if (fleshDamage > 0)// track if perfect fight was runied if we took dmg
        {
            tookFleshDamageThisBattle = true;
        }
    }
    private void HandleEnemyTookDamage(int amount)
    {
        damageDealtThisTurn += amount;
    }
    private void EvaluateTurnDamage()
    {
        if (damageDealtThisTurn > runState.runStats.highestSingleTurnDamage) // check if we beat high score
        {
            runState.runStats.highestSingleTurnDamage = damageDealtThisTurn;
        }

        // Empty the bucket for the next turn!
        damageDealtThisTurn = 0;
    }


    private void HandlePlayerDeath()
    {
        isTrackingTime = false; // stop clock player is dead
    }
}