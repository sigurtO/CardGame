using System.Collections.Generic;
using UnityEngine;

// This line adds a new button to your right-click menu in Unity!
[CreateAssetMenu(fileName = "New Encounter", menuName = "Game Data/Encounter Definition")]
public class EncounterDefinition : ScriptableObject
{
    [Header("Encounter Info")]
    public string encounterName; // e.g., "The Goblin Ambush"

    // Optional: Good for UI if you want to show a skull icon for Elites!
    public EncounterType type = EncounterType.Normal;

    [Header("The Enemies")]
    [Tooltip("Drag your Enemy Prefabs here in the exact order you want them to spawn!")]
    public List<GameObject> enemiesToSpawn = new List<GameObject>();

    // Optional: Add rewards here later!
    // [Header("Rewards")]
    // public int goldReward = 15;
    // public List<CardData> cardDropPool;
}

// A simple enum to help categorize your encounters later
public enum EncounterType
{
    Normal,
    Elite,
    Boss,
    Treasure // (If you want non-combat rooms later!)
}