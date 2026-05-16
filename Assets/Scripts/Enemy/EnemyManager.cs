using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class EnemyManager : MonoBehaviour
{
    [Header("Depencies")]
    [SerializeField] private GameObject playerTarget;


    public List<Enemy> activeEnemies = new List<Enemy>();

    public static event Action OnBattleWon;


    private void OnEnable()
    {
        Enemy.OnEnemyDied += HandleEnemyDeath;
    }

    private void OnDisable()
    {
        Enemy.OnEnemyDied -= HandleEnemyDeath;
    }

    public void RollEnemyIntent()
    {
        foreach (Enemy enemy in activeEnemies)
        {
            enemy.ChoosePhase();
        }

        Debug.Log("[EnemyManager] All enemies have rolled their intents.");
    }

    public IEnumerator ExecuteEnemyTurn(System.Action OnEnemyTurnEnded)
    {
        Debug.Log("[EnemyManager] Enemy Turn Started!");

        // FIX: Loop backwards so we can safely remove destroyed/null enemies
        for (int i = activeEnemies.Count - 1; i >= 0; i--)
        {
            Enemy currentEnemy = activeEnemies[i];

            // 1. Enemy was destroyed earlier? (dead, despawned, etc.)
            if (currentEnemy == null)
            {
                activeEnemies.RemoveAt(i);
                continue;
            }

            // 2. Enemy is alive → execute its saved intent
            currentEnemy.ExecutePhase(playerTarget);

            // 3. Optional pacing delay
            yield return new WaitForSeconds(1f);
        }

        // 4. Notify the turn system that enemies are done
        OnEnemyTurnEnded?.Invoke();
    }


    public void CheckForBattleEnd()
    {
        if (activeEnemies.Count == 0)
        {
            Debug.Log("All enemies dead! We won!");
            OnBattleWon?.Invoke(); // Shout to the game that we won!
        }
    }

    private void HandleEnemyDeath(Enemy deadEnemy)
    {
        // 1. Remove the dead enemy from our list instantly
        if (activeEnemies.Contains(deadEnemy))
        {
            activeEnemies.Remove(deadEnemy);
        }

        // 2. Check if that was the last one!
        CheckForBattleEnd();
    }

}
