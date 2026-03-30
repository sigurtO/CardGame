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

        // Loop through all alive enemies
        for (int i = activeEnemies.Count - 1; i >= 0; i--)
        {
            Enemy currentEnemy = activeEnemies[i];

            // Tell the enemy to fire whatever it saved in its CurrentIntent!
            currentEnemy.ExecutePhase(playerTarget);

            // Wait 1 second before the next enemy attacks
            yield return new WaitForSeconds(1f);
        }

        OnEnemyTurnEnded?.Invoke();
    }



    public void RemoveEnemy(Enemy enemyDied)
    {
        activeEnemies.Remove(enemyDied);

        if (activeEnemies.Count == 0)
        {
            Debug.Log("YOU WIN THE BATTLE!");
            // Trigger victory screen!
        }
    }

}
