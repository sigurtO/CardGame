using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Encounter", menuName = "Game Data/Encounter Definition")]
public class EncounterDefinition : ScriptableObject
{
    [Header("Encounter Info")]
    public string encounterName; 

    public EncounterType type = EncounterType.Normal;

    [Header("The Enemies")]
    [Tooltip("Drag your Enemy Prefabs here in the order you want them to spawn!")]
    public List<GameObject> enemiesToSpawn = new List<GameObject>();

}

public enum EncounterType
{
    Normal,
    Elite,
    Boss,
    Treasure 
}