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

        for (int i = activeEnemies.Count - 1; i >= 0; i--)
        {
            Enemy currentEnemy = activeEnemies[i];


            if (currentEnemy == null) // make sure enemy is removed
            {
                activeEnemies.RemoveAt(i);
                continue;
            }

            currentEnemy.ExecutePhase(playerTarget);

            yield return new WaitForSeconds(1f); //delay for multiple enemies
        }

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
        if (activeEnemies.Contains(deadEnemy))
        {
            activeEnemies.Remove(deadEnemy);
        }

        CheckForBattleEnd();
    }

}
