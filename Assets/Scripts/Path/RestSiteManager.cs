using UnityEngine;

public class RestSiteManager : MonoBehaviour
{
    [Header("Data Backpack")]
    [SerializeField] private CurrentRunState runState;

    [Header("UI References")]
    [SerializeField] private GameObject restSitePanel; // The UI overlay
    [SerializeField] private MapManager mapManager;    // To tell the map to unlock the next row

    [Header("Settings")]
    [Tooltip("How much max HP to heal (0.3 = 30%)")]
    [SerializeField] private float healPercentage = 0.3f;

    public void ShowRestSite()
    {
        restSitePanel.SetActive(true);
    }

    public void OnRestButtonClicked()
    {
        int healAmount = Mathf.RoundToInt(runState.currentMaxHealth * healPercentage);

        runState.currentHealth += healAmount; //save hp to run state

        runState.currentHealth = Mathf.Min(runState.currentHealth, runState.currentMaxHealth); // cap at max health

        Debug.Log($"[RestSite] Healed for {healAmount}! HP is now {runState.currentHealth}/{runState.currentMaxHealth}");

        restSitePanel.SetActive(false);

        mapManager.UpdateMapVisuals();
    }
}