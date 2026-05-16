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

    // Called by the MapManager when a Rest Node is clicked
    public void ShowRestSite()
    {
        restSitePanel.SetActive(true);
        // Optional: Play a nice campfire sound effect here!
    }

    // Hook this up to a "Rest" UI Button
    public void OnRestButtonClicked()
    {
        // 1. Calculate the heal amount (e.g., 30% of max health)
        int healAmount = Mathf.RoundToInt(runState.currentMaxHealth * healPercentage);

        // 2. Add it to the Backpack
        runState.currentHealth += healAmount;

        // 3. Clamp it so we don't over-heal past Max HP
        runState.currentHealth = Mathf.Min(runState.currentHealth, runState.currentMaxHealth);

        Debug.Log($"[RestSite] Healed for {healAmount}! HP is now {runState.currentHealth}/{runState.currentMaxHealth}");

        // 4. Close the UI
        restSitePanel.SetActive(false);

        // 5. Tell the MapManager to unlock the paths above this node!
        mapManager.UpdateMapVisuals();
    }
}