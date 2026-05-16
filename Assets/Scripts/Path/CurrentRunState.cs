using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Data/Current Run State")]
public class CurrentRunState : ScriptableObject
{
    public EncounterDefinition nextEncounter;
    public string currentNodeID;

    [Header("Analytics")]
    public RunStatistics runStats = new RunStatistics();

    public List<CardData> masterDeck = new List<CardData>();


    public int startingMaxHealth = 100;

    [Tooltip("The cards the player ALWAYS starts a new run with.")]
    public List<CardData> starterDeck = new List<CardData>();
    [Tooltip("The cards the player can win (could be class specific)")]
    public CardPool classCardPool;    // reward cards


    public int currentHealth;
    public int currentMaxHealth;

    public void ResetRun()
    {
        currentNodeID = string.Empty;
        nextEncounter = null;

        masterDeck.Clear();
        masterDeck.AddRange(starterDeck);
        runStats.Reset(); // reset stats for new run

        currentMaxHealth = startingMaxHealth;
        currentHealth = startingMaxHealth;

        Debug.Log("CurrentRunState has been reset for new playthrough");
    }
}