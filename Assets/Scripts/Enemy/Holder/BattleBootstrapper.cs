using System.Collections;
using UnityEngine;

public class BattleBootstrapper : MonoBehaviour
{
    [Header("Data Backpack")]
    [SerializeField] private CurrentRunState runState;

    [Header("Managers")]
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private TurnManager turnManager;

    [Header("Spawning Settings")]
    [SerializeField] private Transform centerSpawnPoint;
    [SerializeField] private float spacing = 3f;

    private void Start()
    {
        // Start the Coroutine instead of a normal method
        StartCoroutine(SetupBattleRoutine());
    }

    // Notice the IEnumerator!
    private IEnumerator SetupBattleRoutine()
    {
        if (runState == null || runState.nextEncounter == null)
        {
            Debug.LogError("[BattleBootstrapper] No Encounter data found!");
            yield break; // Stop the coroutine
        }

        EncounterDefinition encounter = runState.nextEncounter;
        int enemyCount = encounter.enemiesToSpawn.Count;
        float startX = -((enemyCount - 1) * spacing) / 2f;

        // 1. Spawn the Enemies
        for (int i = 0; i < enemyCount; i++)
        {
            Vector3 spawnPos = centerSpawnPoint.position + new Vector3(startX + (i * spacing), 0, 0);
            GameObject spawnedEnemy = Instantiate(encounter.enemiesToSpawn[i], spawnPos, Quaternion.identity);

            Enemy enemyScript = spawnedEnemy.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyManager.activeEnemies.Add(enemyScript);
            }
        }

        // 2. THE MAGIC FIX: Wait exactly 1 frame. 
        // This gives all the newly spawned Enemies time to run their Start() methods!
        yield return null;

        // 3. The dust has settled. Fire the starting gun!
        // (Make sure this matches your method name, your log said BattleStart!)
        turnManager.BattleStart();
    }
}