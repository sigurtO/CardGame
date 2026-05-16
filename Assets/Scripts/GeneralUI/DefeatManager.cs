using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DefeatManager : MonoBehaviour
{
    [Header("Data Backpack")]
    [SerializeField] private CurrentRunState runState; // So we can reset the run!
    [SerializeField] private UmbracoService umbracoService; // Inject the new service!

    [Header("UI References")]
    [SerializeField] private GameObject defeatScreenPanel;

    [Header("Stat Text References")]
    [SerializeField] private TextMeshProUGUI encountersSurvivedText;
    [SerializeField] private TextMeshProUGUI enemiesKilledText;
    [SerializeField] private TextMeshProUGUI damageTakenText;
    [SerializeField] private TextMeshProUGUI damageAbsorbedText;
    [SerializeField] private TextMeshProUGUI highestDamageText;
    [SerializeField] private TextMeshProUGUI perfectFightsText;
    [SerializeField] private TextMeshProUGUI timeSpentText;

    private void OnEnable()
    {
        Player.OnDeath += ShowDefeatScreen;
    }

    private void OnDisable()
    {
        // Always unsubscribe to prevent memory leaks!
        Player.OnDeath -= ShowDefeatScreen;
    }

    private void ShowDefeatScreen()
    {
        PopulateRunStats();


        defeatScreenPanel.SetActive(true);
    }

    private void PopulateRunStats()
    {
        // Safety check
        if (runState == null || runState.runStats == null) return;

        RunStatistics stats = runState.runStats;

        if (encountersSurvivedText != null) encountersSurvivedText.text = "Encounters Survived: " + stats.encountersSurvived.ToString();
        if (enemiesKilledText != null) enemiesKilledText.text = "Enemies Killed: " + stats.enemiesKilled.ToString();
        if (damageTakenText != null) damageTakenText.text = "Damage Taken: " + stats.totalDamageTaken.ToString();
        if (damageAbsorbedText != null) damageAbsorbedText.text = "Damage Absorbed: " + stats.damageAbsorbed.ToString();
        if (highestDamageText != null) highestDamageText.text = "Highest Damage: " + stats.highestSingleTurnDamage.ToString();
        if (perfectFightsText != null) perfectFightsText.text = "Perfect Fights: " + stats.perfectFights.ToString();
        if (timeSpentText != null)
        {
            TimeSpan time = TimeSpan.FromSeconds(stats.timeSpentInBattle);
            timeSpentText.text = "Time Spent: " + time.ToString(@"mm\:ss");
        }
    }

    // Hook this up to your "Return to Map" UI Button OnClick event!
    public void ReturnToMapButtonClicked()
    {
        Debug.Log("[DefeatManager] 'Return to Map' clicked. Delegating to Umbraco Service...");

        if (runState != null && runState.runStats != null && umbracoService != null)
        {
            // 1. INJECT UMBRACO META DATA!
            // Grab the name from PlayerPrefs (Default to Guest if they haven't set a name yet)
            runState.runStats.playerName = PlayerPrefs.GetString("CurrentPlayerName", "Guest_1234");

            // Stamp it with the exact current time in UTC
            runState.runStats.runDate = System.DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");

            // 2. Generate the perfectly formatted JSON
            string jsonPayload = runState.runStats.ToJson();
            Debug.Log("<color=cyan><b>--- PREPARING EXPORT ---</b></color>\n" + jsonPayload);

            // 3. Hand it to the service, and pass ExecuteSceneTransition as the callback!
            umbracoService.SendRunData(jsonPayload, ResetRunAfterUmbracoTransfer);
        }
        else
        {
            Debug.LogWarning("[DefeatManager] Missing RunState or UmbracoService! Skipping API call.");
            ResetRunAfterUmbracoTransfer();
        }
    }

    private void ResetRunAfterUmbracoTransfer()
    {
        if (runState != null) runState.ResetRun();

        SceneManager.LoadScene("MapScene");
    }
}