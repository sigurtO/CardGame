using NUnit.Framework.Interfaces;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum NodeType
{
    Encounter, //battle
    RestSite, //heal
    Shop, // maybe add later
    Mystery // maybe add later
}

public class MapNode : MonoBehaviour
{
    [Header("The Encounter")]
    [SerializeField] private EncounterDefinition myEncounter;

    [Header("Map Connections")]
    public List<MapNode> nextNodes = new List<MapNode>();

    [Header("Node Identity")]
    public string nodeID; // to member where we are in the run
    public NodeType nodeType = NodeType.Encounter;

    [Header("UI Components")]
    [SerializeField] private Button nodeButton;
    [SerializeField] private Image nodeIcon;


    private void Awake()
    {
        if (nodeButton == null) nodeButton = GetComponent<Button>();
    }

    // Called by the MapManager to turn this specific node on or off
    public void SetInteractable(bool isClickable)
    {
        nodeButton.interactable = isClickable;

        // Optional: Dim the color if it's locked so the player knows!
        nodeIcon.color = isClickable ? Color.white : Color.gray;
    }
    

    // A getter so the MapManager can see what encounter this node holds
    public EncounterDefinition GetEncounter() => myEncounter;
}